using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings.Cryptography;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.Findings.Authentication;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class PasswordSignInAnalyzer
{
    public static List<BaseFinding> FindDisabledLockouts(SyntaxNode root)
    {
        var walker = new PasswordSignInSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var signIn in walker.MethodCalls)
        {
            try
            {
                if (signIn.ArgumentList.Arguments.Count == 4)
                {
                    if (signIn.ArgumentList.Arguments[3].Expression is LiteralExpressionSyntax literal)
                    {
                        var value = literal.GetLiteralValue<bool>();

                        if (!value)
                        {
                            var finding = new PasswordSignInMissingLockout();
                            finding.RootLocation = new SourceLocation(signIn);
                            findings.Add(finding);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(signIn, ex));
            }
        }

        return findings;
    }
}
