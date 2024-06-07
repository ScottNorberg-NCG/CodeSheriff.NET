using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Cryptography;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class HashAlgorithmAnalyzer
{
    public static List<BaseFinding> FindDeprecatedAlgorithms(SyntaxNode root)
    {
        var walker = new ComputeHashSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var algorithm in walker.MethodCalls)
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
