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
    internal static class ExpressionSyntaxExtensions
    {
        internal static ISymbol ToSymbol(this ExpressionSyntax expression)
        {
            var model = Globals.Compilation.GetSemanticModel(expression.SyntaxTree);
            return model.GetSymbolInfo(expression).Symbol;
        }

        internal static SyntaxNode GetDefinitionNode(this ExpressionSyntax expression, SyntaxNode root)
        {
            var asSymbol = expression.ToSymbol();
            var definition = SymbolFinder.FindSourceDefinitionAsync(asSymbol, Globals.Solution).Result;

            return root.FindNode(definition.Locations.First().SourceSpan);
        }

        internal static ITypeSymbol GetUnderlyingType(this ExpressionSyntax expression)
        {
            var asSymbol = expression.ToSymbol();

            if (asSymbol is ILocalSymbol localSymbol)
            {
                return localSymbol.Type;
            }
            else if (asSymbol is IParameterSymbol parameterSymbol)
            {
                return parameterSymbol.Type; 
            }
            else if (asSymbol is IFieldSymbol fieldSymbol)
            {
                return fieldSymbol.Type;
            }
            else if (asSymbol is IPropertySymbol propertySymbol)
            {
                return propertySymbol.Type;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
