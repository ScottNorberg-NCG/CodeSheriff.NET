using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.Cryptography;
using Opperis.SAST.Engine.Findings.ProgramFlow;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers
{
    internal static class ExternalRedirectAnalyzer
    {
        internal static List<BaseFinding> FindProblematicExternalRedirects(ExternalRedirectSyntaxWalker walker, SyntaxNode root)
        {
            if (walker.UnvalidatedRedirects.Count == 0)
                walker.Visit(root);

            var findings = new List<BaseFinding>();

            foreach (var redirect in walker.UnvalidatedRedirects)
            {
                foreach (var arg in redirect.ArgumentList.Arguments.Select(a => a.Expression))
                {
                    if (arg is IdentifierNameSyntax)
                    {
                        AddIfStringParameter(root, findings, arg);
                    }
                    else if (arg is BinaryExpressionSyntax binary)
                    {
                        foreach (var expression in binary.GetNonLiteralPortions())
                        {
                            //TODO: Handle this
                            if (expression is InvocationExpressionSyntax)
                                continue;

                            AddIfStringParameter(root, findings, expression);
                        }
                    }
                    else if (arg is InterpolatedStringExpressionSyntax interpolated)
                    {
                        foreach (var expression in interpolated.GetNonLiteralPortions())
                        {
                            //TODO: Handle this
                            if (expression is InvocationExpressionSyntax)
                                continue;

                            AddIfStringParameter(root, findings, expression);
                        }
                    }
                    else if (arg is LiteralExpressionSyntax)
                    {
                        //Do nothing, not a finding
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }

            return findings;
        }

        private static void AddIfStringParameter(SyntaxNode root, List<BaseFinding> findings, ExpressionSyntax? arg)
        {
            var underlyingType = arg.GetUnderlyingType();

            if (underlyingType != null && underlyingType.ToDisplayString() == "string" && ValueCameFromExternallyFacingMethodParam(arg, root))
            {
                var finding = new UnprotectedExternalRedirect();
                finding.RootLocation = new SourceLocation(arg);

                findings.Add(finding);
            }
        }

        private static bool ValueCameFromExternallyFacingMethodParam(ExpressionSyntax arg, SyntaxNode root)
        {
            var definition = arg.GetDefinitionNode(root);

            if (definition is ParameterSyntax parameter)
            {
                //parameter.Parent is probably a ParameterList

                if (definition.Parent.Parent is MethodDeclarationSyntax method) 
                {
                    if (method.IsUIProcessor())
                        return true;
                }
            }

            return false;
        }
    }
}
