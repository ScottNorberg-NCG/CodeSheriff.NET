using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SAST.Engine.RoslynObjectExtensions;

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

                LookForTraditionalSaves(accessPoints, method);
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

                LookForTraditionalSaves(accessPoints, method);
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(method, ex));
            }
        }

        return accessPoints;
    }

    private static void LookForTraditionalSaves(List<DataAccessItem> accessPoints, MethodDeclarationSyntax method)
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
                    //Read of some sort
                    //TODO: later 
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
                    }
                }
                else
                {

                }
            }
        }
    }
}
