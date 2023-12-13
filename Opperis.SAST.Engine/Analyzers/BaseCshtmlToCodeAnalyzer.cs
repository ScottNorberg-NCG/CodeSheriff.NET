using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.CompiledCSHtmlParsing;
using Opperis.SAST.Engine.Findings.XSS;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Opperis.SAST.Engine.Analyzers;

//This class is kind of awkward. The other analyzers are all static classes, but because this is an abstract class it is not static.
//A refactor is in order, but to what I'm not sure
internal abstract class BaseCshtmlToCodeAnalyzer
{
    protected abstract BaseFinding GetNewFindingForControllerAndGet();
    protected abstract BaseFinding GetNewFindingForControllerAndPost();

    protected void GetFindingsForCshtmlInvocation(List<BaseFinding> findings, InvocationExpressionSyntax call)
    {
        var syntaxTreeString = call.SyntaxTree.GetText().ToString();
        var methodInfo = SyntaxTreeParser.Parse(syntaxTreeString);

        var possibleMethods = Globals.SolutionControllerMethods.Where(m => m.HasName(methodInfo.Controller, methodInfo.Method)).ToList();

        var argToHtmlRaw = call.ArgumentList.Arguments.Single();

        if (argToHtmlRaw.Expression is MemberAccessExpressionSyntax htmlRawArgAsMemberAccess)
        {
            if (htmlRawArgAsMemberAccess.Expression is IdentifierNameSyntax htmlRawArgIdentifier)
            {
                var htmlRawSemanticModel = Globals.Compilation.GetSemanticModel(htmlRawArgIdentifier.SyntaxTree);
                var htmlRawArgSymbol = htmlRawSemanticModel.GetSymbolInfo(htmlRawArgIdentifier).Symbol;
                var htmlRawArgType = htmlRawArgSymbol.ContainingType.TypeArguments.FirstOrDefault();
                var controllerMethodSuspects = possibleMethods.SelectMany(m => m.ReturnsModelType(htmlRawArgType)).ToList();

                foreach (var suspect in controllerMethodSuspects)
                {
                    if (suspect.ContainingMethod.ParameterList.Parameters.Count == 0)
                        continue;

                    if (suspect.ContainingMethod.ParameterList.Parameters.Any(p => p.Type.GetUnderlyingType() == htmlRawArgType))
                    {
                        var allowedMethods = suspect.ContainingMethod.GetMethodVerbs();

                        BaseFinding finding;

                        if (allowedMethods.Count == 0 || allowedMethods.Any(a => a.Method == MethodDeclarationSyntaxExtensions.HttpMethodInfo.HttpMethod.Get))
                            finding = GetNewFindingForControllerAndGet();
                        else
                            finding = GetNewFindingForControllerAndPost();

                        finding.RootLocation = new SourceLocation(argToHtmlRaw);

                        var callStack = new CallStack();
                        callStack.AddLocation(call);
                        callStack.AddLocation(suspect.ReturnObject);
                        callStack.AddLocation(suspect.ContainingMethod.ParameterList.Parameters.First(p => p.Type.GetUnderlyingType() == htmlRawArgType));
                        callStack.AddLocation(suspect.ContainingMethod);
                        finding.CallStacks.Add(callStack);

                        findings.Add(finding);
                    }

                    var htmlRawProperty = htmlRawArgAsMemberAccess.Name as IdentifierNameSyntax;

                    if (htmlRawProperty != null)
                    {
                        foreach (var node in suspect.ContainingMethod.DescendantNodes().Where(e => e is MemberAccessExpressionSyntax))
                        {
                            var member = node as MemberAccessExpressionSyntax;

                            var nodeType = member.Expression.GetUnderlyingType();

                            if (nodeType != null)
                            {
                                if (nodeType.Equals(htmlRawArgType) && member.Name.Identifier.Text == htmlRawProperty.ToString())
                                {
                                    if (member.Parent is AssignmentExpressionSyntax assignment)
                                    {
                                        var source = assignment.Right.GetDefinitionNode(suspect.ContainingMethod);

                                        if (source is ParameterSyntax parameter)
                                        {
                                            var allowedMethods = suspect.ContainingMethod.GetMethodVerbs();

                                            BaseFinding finding;

                                            if (allowedMethods.Count == 0 || allowedMethods.Any(a => a.Method == MethodDeclarationSyntaxExtensions.HttpMethodInfo.HttpMethod.Get))
                                                finding = GetNewFindingForControllerAndGet();
                                            else
                                                finding = GetNewFindingForControllerAndPost();

                                            finding.RootLocation = new SourceLocation(argToHtmlRaw);

                                            var callStack = new CallStack();
                                            callStack.AddLocation(call);
                                            callStack.AddLocation(suspect.ReturnObject);
                                            callStack.AddLocation(assignment);
                                            callStack.AddLocation(source);
                                            callStack.AddLocation(suspect.ContainingMethod);
                                            finding.CallStacks.Add(callStack);

                                            findings.Add(finding);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
