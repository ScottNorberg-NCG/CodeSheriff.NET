using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Cryptography;
using CodeSheriff.SAST.Engine.Findings.ProgramFlow;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class ExternalRedirectAnalyzer
{
    public static List<BaseFinding> FindProblematicExternalRedirects(SyntaxNode root)
    {
        var walker = new ExternalRedirectSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var redirect in walker.UnvalidatedRedirects)
        {
            try
            {
                foreach (var arg in redirect.ArgumentList.Arguments.Select(a => a.Expression))
                {
                    if (arg is IdentifierNameSyntax)
                    {
                        AddIfStringParameter(root, findings, arg, arg);
                    }
                    else if (arg is BinaryExpressionSyntax binary)
                    {
                        foreach (var expression in binary.GetNonLiteralPortions())
                        {
                            //TODO: Handle this
                            if (expression is InvocationExpressionSyntax)
                                continue;

                            AddIfStringParameter(root, findings, expression, expression);
                        }
                    }
                    else if (arg is InterpolatedStringExpressionSyntax interpolated)
                    {
                        foreach (var expression in interpolated.GetNonLiteralPortions())
                        {
                            //TODO: Handle this
                            if (expression is InvocationExpressionSyntax)
                                continue;

                            AddIfStringParameter(root, findings, expression, expression);
                        }
                    }
                    else if (arg is MemberAccessExpressionSyntax member)
                    {
                        AddIfStringParameter(root, findings, member, member.Expression);
                    }
                    else if (arg is LiteralExpressionSyntax)
                    {
                        //Do nothing, not a finding
                    }
                    else
                    {
                        //Do nothing for now
                        //throw new NotSupportedException();
                    }
                }
            }
            catch (Exception ex) 
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(redirect, ex));
            }
        }

        return findings;
    }

    private static void AddIfStringParameter(SyntaxNode root, List<BaseFinding> findings, ExpressionSyntax? redirectArgument, ExpressionSyntax? redirectContainer)
    {
        var underlyingType = redirectArgument.GetUnderlyingType();

        if (underlyingType != null && underlyingType.ToDisplayString().In("string", "string?") && 
            ValueCameFromExternallyFacingMethodParam(redirectArgument, root) || ValueCameFromExternallyFacingMethodParam(redirectContainer, root))
        {
            var finding = new UnprotectedExternalRedirect();
            finding.RootLocation = new SourceLocation(redirectArgument);

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
