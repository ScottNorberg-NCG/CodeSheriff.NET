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

namespace CodeSheriff.SAST.Engine.Analyzers;

public class RSAKeySizeInPropertyAnalyzer
{
    public static List<BaseFinding> FindInadequateKeyLengths(SyntaxNode root)
    {
        var walker = new RSAKeySizeSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var rsa in walker.KeyLengthSets)
        {
            try
            {
                var parent = rsa.Parent as AssignmentExpressionSyntax;

                if (parent == null)
                    continue;
                else if (parent.Right is LiteralExpressionSyntax literal)
                {
                    var value = Convert.ToInt32(literal.Token.Value);

                    if (value < 2048)
                        findings.Add(new RSAWithInadequateKeyLength(rsa));
                }
                else if (parent.Right is IdentifierNameSyntax identifier)
                {
                    var value = identifier.GetLiteralValue<int>();

                    if (value > 0 && value < 2048)
                        findings.Add(new RSAWithInadequateKeyLength(rsa));
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(rsa, ex));
            }
        }

        return findings;
    }
}
