using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings.Cryptography;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.Findings.Authentication;

namespace Opperis.SAST.Engine.Analyzers;

internal static class PasswordSignInAnalyzer
{
    internal static List<BaseFinding> FindDisabledLockouts(PasswordSignInSyntaxWalker walker, SyntaxNode root)
    {
        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var signIn in walker.SignIns)
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
