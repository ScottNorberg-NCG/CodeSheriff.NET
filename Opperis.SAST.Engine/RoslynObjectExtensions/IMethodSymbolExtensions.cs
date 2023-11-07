using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class IMethodSymbolExtensions
    {
        internal static bool IsUIProcessor(this IMethodSymbol symbol)
        {
            var method = symbol.DeclaringSyntaxReferences.First().GetSyntax() as MethodDeclarationSyntax;

            if (method == null)
                return false;

            return method.IsUIProcessor();
        }

        internal static List<CallStack> CrawlTrees(this IMethodSymbol method, CallStack stack)
        {
            var callStacks = new List<CallStack>();

            var references = method.GetMethodsReferencedIn();

            if (!stack.Locations.Last().Equals(new SourceLocation(method)))
                stack.Locations.Add(new SourceLocation(method));

            if (references.Count == 0)
            {
                stack.Locations.Add(new SourceLocation(method.ContainingSymbol));
                callStacks.Add(stack);
            }
            else if (references.Count == 1)
            {
                foreach (var callStack in CrawlTrees(references.Single(), stack))
                {
                    callStacks.Add(callStack);
                }
            }
            else
            {
                foreach (var reference in references)
                {
                    foreach (var callStack in CrawlTrees(reference, stack.Clone()))
                    {
                        callStacks.Add(callStack);
                    }
                }
            }

            return callStacks;
        }

        internal static List<IMethodSymbol> GetMethodsReferencedIn(this IMethodSymbol methodSymbol)
        {
            var toReturn = new List<IMethodSymbol>();

            var references = SymbolFinder.FindReferencesAsync(methodSymbol, Globals.Solution).Result;

            foreach (var reference in references)
            {
                foreach (var referenceLocation in reference.Locations)
                {
                    //var referenceDocument = Globals.Solution.GetDocument(referenceLocation.Document.Id);
                    //var referenceRoot = referenceDocument.GetSyntaxRootAsync().Result;

                    var referenceRoot = referenceLocation.Document.GetSyntaxRootAsync().Result;

                    // Find the syntax node where the method is called
                    var referenceNode = referenceRoot.FindNode(referenceLocation.Location.SourceSpan);

                    SyntaxNode? parent = referenceNode.Parent;
                    
                    while (parent != null && !(parent is MethodDeclarationSyntax))
                    {
                        parent = parent.Parent;
                    }

                    if (parent != null) 
                    { 
                        var semanticModel = Globals.Compilation.GetSemanticModel(parent.SyntaxTree);
                        var symbol = (IMethodSymbol)semanticModel.GetDeclaredSymbol(parent);

                        toReturn.Add(symbol);                    
                    }
                }
            }

            return toReturn;
        }
    }
}
