using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Opperis.SAST.Engine.CompiledCSHtmlParsing;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.CSRF;
using Opperis.SAST.Engine.Findings.XSS;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers
{
    internal static class HtmlRawAnalyzer
    {
        internal static List<BaseFinding> FindXssIssues(HtmlRawSyntaxWalker walker, SyntaxNode root)
        {
            if (!walker.HtmlRawCalls.Any())
                walker.Visit(root);

            var findings = new List<BaseFinding>();

            foreach (var call in walker.HtmlRawCalls)
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
                                    finding = new BindObjectForGetUsedInHtmlRaw();
                                else
                                    finding = new BindObjectForOtherMethodUsedInHtmlRaw();

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
                                                        finding = new BindObjectForGetUsedInHtmlRaw();
                                                    else
                                                        finding = new BindObjectForOtherMethodUsedInHtmlRaw();

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

            return findings;
        }

        private static void SetFinding(BaseFinding finding, MethodDeclarationSyntax method)
        {
            var callStack = new CallStack();
            callStack.Locations.Add(new SourceLocation(method));
            callStack.Locations.Add(new SourceLocation(method.Parent));
            finding.CallStacks.Add(callStack);

            finding.RootLocation = new SourceLocation(method);
        }
    }
}
