using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.Cryptography;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers
{
    internal static class SymmetricAlgorithmAnalyzer
    {
        internal static List<BaseFinding> FindDeprecatedAlgorithms(SymmetricAlgorithmSyntaxWalker walker, SyntaxNode root)
        {
            if (walker.SymmetricAlgorithms.Count == 0)
                walker.Visit(root);

            var findings = new List<BaseFinding>();

            foreach (var algorithm in walker.SymmetricAlgorithms)
            {
                var identifierName = algorithm.Expression as IdentifierNameSyntax;

                var semanticModel = Globals.Compilation.GetSemanticModel(identifierName.SyntaxTree);

                var symbol = semanticModel.GetSymbolInfo(identifierName).Symbol as ILocalSymbol;

                var type = symbol.Type;

                if (!type.Name.StartsWith("Aes") && !type.Name.StartsWith("Rijndael"))
                {
                    var finding = new UseOfDeprecatedAlgorithm();

                    var callStack = new CallStack();
                    callStack.Locations.Add(new SourceLocation(algorithm));
                    finding.CallStacks.Add(callStack);

                    finding.AdditionalInformation = $"Algorithm found: {type.Name}";

                    findings.Add(finding);
                }
            }

            return findings;
        }
    }
}
