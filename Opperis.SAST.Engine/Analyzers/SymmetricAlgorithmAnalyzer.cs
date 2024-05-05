using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Cryptography;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

internal static class SymmetricAlgorithmAnalyzer
{
    internal static List<BaseFinding> FindDeprecatedAlgorithms(SyntaxNode root)
    {
        var walker = new SymmetricAlgorithmSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var algorithm in walker.SymmetricAlgorithms)
        {
            try
            {
                var identifierName = algorithm.Expression as IdentifierNameSyntax;

                var semanticModel = Globals.Compilation.GetSemanticModel(identifierName.SyntaxTree);

                var symbol = semanticModel.GetSymbolInfo(identifierName).Symbol as ILocalSymbol;

                var type = symbol.Type;

                if (!type.Name.StartsWith("Aes") && !type.Name.StartsWith("Rijndael"))
                {
                    var finding = new UseOfDeprecatedSymmetricAlgorithm();
                    finding.RootLocation = new SourceLocation(algorithm);

                    finding.AdditionalInformation = $"Algorithm found: {type.Name}";

                    findings.Add(finding);
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
