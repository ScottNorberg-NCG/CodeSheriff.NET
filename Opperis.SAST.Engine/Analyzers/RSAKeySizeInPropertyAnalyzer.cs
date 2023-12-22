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

namespace Opperis.SAST.Engine.Analyzers;

internal class RSAKeySizeInPropertyAnalyzer
{
    internal static List<BaseFinding> FindInadequateKeyLengths(RSAKeySizeSyntaxWalker walker, SyntaxNode root)
    {
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
