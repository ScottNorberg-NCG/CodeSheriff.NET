using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.Cryptography;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers;

internal static class HashAlgorithmAnalyzer
{
    internal static List<BaseFinding> FindDeprecatedAlgorithms(ComputeHashSyntaxWalker walker, SyntaxNode root)
    {
        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var algorithm in walker.ComputeHashCalls)
        {
            try
            {
                var memberAccess = algorithm.Expression as MemberAccessExpressionSyntax;
                var identifier = memberAccess.Expression as IdentifierNameSyntax;

                var type = identifier.GetUnderlyingType();

                if (type != null)
                {
                    var typeName = type.Name.Replace("?", "");

                    if (typeName.StartsWith("SHA1") || typeName.StartsWith("MD5"))
                    {
                        var finding = new UseOfDeprecatedHashAlgorithm();
                        finding.RootLocation = new SourceLocation(algorithm);

                        finding.AdditionalInformation = $"Algorithm found: {type.Name}";

                        findings.Add(finding);
                    }
                }
            }
            catch (Exception ex) 
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(algorithm, ex));
            }
        }

        return findings;
    }
}
