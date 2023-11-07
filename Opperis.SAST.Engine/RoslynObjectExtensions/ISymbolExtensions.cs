using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class ISymbolExtensions
    {
        internal static List<ExpressionSyntax> GetReferenceExpressions(this ISymbol symbol)
        {
            var toReturn = new List<ExpressionSyntax>();

            var references = SymbolFinder.FindReferencesAsync(symbol, Globals.Solution).Result;

            foreach (var reference in references)
            {
                foreach (var referenceLocation in reference.Locations)
                {
                    //var referenceDocument = Globals.Solution.GetDocument(referenceLocation.Document.Id);
                    //var referenceRoot = referenceDocument.GetSyntaxRootAsync().Result;

                    var referenceRoot = referenceLocation.Document.GetSyntaxRootAsync().Result;

                    // Find the syntax node where the method is called
                    var referenceNode = referenceRoot.FindNode(referenceLocation.Location.SourceSpan);

                    if (referenceNode is ExpressionSyntax expression)
                        toReturn.Add(expression);
                    //SyntaxNode? parent = referenceNode.Parent;

                    //while (parent != null && !(parent is MethodDeclarationSyntax))
                    //{
                    //    parent = parent.Parent;
                    //}

                    //if (parent != null)
                    //{
                    //    var semanticModel = Globals.Compilation.GetSemanticModel(parent.SyntaxTree);
                    //    var symbol = (IMethodSymbol)semanticModel.GetDeclaredSymbol(parent);

                    //    toReturn.Add(symbol);
                    //}
                }
            }

            return toReturn;
        }
    }
}
