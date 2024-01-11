using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Microsoft.CodeAnalysis.FindSymbols;

namespace Opperis.SAST.Engine.DataAccessAnalysis;

internal static class DataAccessAnalyzer
{
    internal static List<DataAccessItem> FindDataAccessPoints()
    {
        var accessPoints = new List<DataAccessItem>();

        if (Globals.EntityFrameworkObjects.Count == 0)
            return accessPoints;

        foreach (var method in Globals.SolutionControllerMethods)
        {
            try
            {
                foreach (var parameter in method.ParameterList.Parameters)
                {
                    foreach (var type in Globals.EntityFrameworkObjects)
                    {
                        if (type.Equals(parameter.Type.GetUnderlyingType()))
                        {
                            var saveCallSyntaxWalker = new EntityFrameworkSaveSyntaxWalker();
                            saveCallSyntaxWalker.Visit(method);

                            if (saveCallSyntaxWalker.MethodCalls.Any())
                            {
                                var callStacks = new List<CallStack>();
                                var callStack = new CallStack();
                                callStack.AddLocation(method);
                                callStacks.Add(callStack);

                                var properties = type.GetProperties() ?? new List<PropertyDeclarationSyntax>();

                                foreach (var prop in properties)
                                {
                                    accessPoints.Add(new DataWrite(method, type, prop.Identifier.Text, callStacks));
                                }
                            }
                        }
                    }
                }

                LookForInMethodAccesses(accessPoints, method);
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(method, ex));
            }
        }

        foreach (var method in Globals.RazorPageMethods)
        {
            try
            {
                var saveCallSyntaxWalker = new EntityFrameworkSaveSyntaxWalker();
                saveCallSyntaxWalker.Visit(method);

                if (saveCallSyntaxWalker.MethodCalls.Any())
                {
                    //We have a method that calls DbContext.Save()
                    //Now look to see if we have any objects that are both [BindObject] and an EF object (Value Shadowing)
                    //TODO: ensure that the bind object was attached to the DbContext object to know for sure whether the object was actually saved
                    var bindObjectWalker = new BindObjectSyntaxWalker();
                    bindObjectWalker.Visit(method);

                    foreach (var bindObject in bindObjectWalker.BindObjectReferences)
                    {
                        if (Globals.EntityFrameworkObjects.Any(ef => ef.Equals(bindObject.AsType)))
                        {
                            var callStacks = new List<CallStack>();
                            var callStack = new CallStack();
                            callStack.AddLocation(method);
                            callStack.AddLocation(bindObject.AsClass);
                            callStacks.Add(callStack);

                            var properties = bindObject.AsType.GetProperties() ?? new List<PropertyDeclarationSyntax>();

                            foreach (var prop in properties)
                            {
                                accessPoints.Add(new DataWrite(method, bindObject.AsType, prop.Identifier.Text, callStacks));
                            }
                        }
                    }
                }

                LookForInMethodAccesses(accessPoints, method);
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(method, ex));
            }
        }

        return accessPoints;
    }

    private static void LookForInMethodAccesses(List<DataAccessItem> accessPoints, MethodDeclarationSyntax method)
    {
        foreach (var child in method.DescendantNodes().Where(c => c is MemberAccessExpressionSyntax).Select(c => c as MemberAccessExpressionSyntax))
        {
            if (child.Expression is IdentifierNameSyntax identifier)
            {
                var objectType = identifier.GetUnderlyingType();

                if (objectType == null || !objectType.IsEntityFrameworkType())
                    continue;

                var parentInvocations = child.Ancestors().Where(a => a is InvocationExpressionSyntax).Select(a => a as InvocationExpressionSyntax).ToList();

                if (parentInvocations.Any(i => i.IsIQueryable()))
                {
                    var assignment = child.Ancestors().FirstOrDefault(a => a is AssignmentExpressionSyntax) as AssignmentExpressionSyntax;

                    if (assignment != null)
                    {
                        if (assignment.Left is MemberAccessExpressionSyntax member)
                        {
                            //Using the ViewBag which makes object information available to the view
                            if (member.Expression.GetUnderlyingType().ToString() == "dynamic")
                            {
                                var callStacks = new List<CallStack>();
                                var callStack = new CallStack();
                                callStack.AddLocation(member.Expression);
                                callStack.AddLocation(method);
                                callStacks.Add(callStack);

                                var properties = objectType.GetProperties() ?? new List<PropertyDeclarationSyntax>();

                                foreach (var prop in properties)
                                {
                                    accessPoints.Add(new IndirectDataExposure(method, objectType, prop.Identifier.Text, callStacks));
                                }
                            }
                            else
                            {
                                if (member.Expression is IdentifierNameSyntax id)
                                {
                                    var references = id.GetReferences();
                                    if (references.Any(r => r.Ancestors().Any(a => a is ReturnStatementSyntax)))
                                    {
                                        var allReturns = references.SelectMany(r => r.Ancestors()).Where(a => a is ReturnStatementSyntax).Select(a => a as ReturnStatementSyntax);

                                        var callStacks = new List<CallStack>();
                                        var callStack = new CallStack();
                                        callStack.AddLocation(id);
                                        callStack.AddLocation(method);
                                        callStacks.Add(callStack);

                                        accessPoints.Add(new IndirectDataExposure(method, objectType, id.Identifier.Text, callStacks));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var variable = child.Ancestors().FirstOrDefault(a => a is VariableDeclaratorSyntax) as VariableDeclaratorSyntax;

                        if (variable != null)
                        {
                            var references = variable.GetReferences();
                            if (references.Any(r => r.Ancestors().Any(a => a is ReturnStatementSyntax)))
                            {
                                var allReturns = references.SelectMany(r => r.Ancestors()).Where(a => a is ReturnStatementSyntax).Select(a => a as ReturnStatementSyntax);

                                var callStacks = new List<CallStack>();
                                var callStack = new CallStack();
                                callStack.AddLocation(variable);
                                callStack.AddLocation(method);
                                callStacks.Add(callStack);

                                if (allReturns.Any(r => r.DescendantNodes().Any(n => n.IsDataExposingInvocation())))
                                {
                                    var properties = objectType.GetProperties() ?? new List<PropertyDeclarationSyntax>();

                                    foreach (var prop in properties)
                                    {
                                        accessPoints.Add(new DataObjectExposedToUI(method, objectType, prop.Identifier.Text, callStacks));
                                    }
                                }
                                else
                                {
                                    accessPoints.Add(new IndirectDataExposure(method, objectType, variable.Identifier.Text, callStacks));
                                }
                            }

                            foreach (var reference in references.Where(r => r.IsAssignmentSourceInTree()))
                            {
                                var varAssignment = reference.Ancestors().Where(r => r is AssignmentExpressionSyntax).Select(r => r as AssignmentExpressionSyntax).First();
                                var destination = varAssignment.Left;

                                if (destination is MemberAccessExpressionSyntax memberAccess)
                                {
                                    if (memberAccess.Expression is IdentifierNameSyntax id)
                                    {
                                        var idReferences = id.GetReferences();

                                        if (idReferences.Any(r => r.Ancestors().Any(a => a is ReturnStatementSyntax)))
                                        {
                                            var allReturns = references.SelectMany(r => r.Ancestors()).Where(a => a is ReturnStatementSyntax).Select(a => a as ReturnStatementSyntax);

                                            var callStacks = new List<CallStack>();
                                            var callStack = new CallStack();
                                            callStack.AddLocation(variable);
                                            callStack.AddLocation(method);
                                            callStacks.Add(callStack);

                                            accessPoints.Add(new IndirectDataExposure(method, objectType, id.Identifier.Text, callStacks));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (child.Parent is AssignmentExpressionSyntax assignment)
                {
                    if (assignment.Left.Equals(child))
                    {
                        var saveCallSyntaxWalker = new EntityFrameworkSaveSyntaxWalker();
                        saveCallSyntaxWalker.Visit(method);

                        if (saveCallSyntaxWalker.MethodCalls.Any(c => c.GetLocation().SourceSpan.Start > assignment.Left.Span.Start))
                        {
                            var callStacks = assignment.Right.GetCallStacks();
                            accessPoints.Add(new DataWrite(method, objectType, child.Name.ToString(), callStacks));
                        }
                    }
                    else
                    {
                        //We're taking EF data and assigning it elsewhere. Probably to the UI?
                        int i = 1;
                    }
                }
                else
                {

                }
            }
        }
    }
}
