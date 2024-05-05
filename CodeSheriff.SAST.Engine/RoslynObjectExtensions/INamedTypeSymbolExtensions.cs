using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.RoslynObjectExtensions;

internal static class INamedTypeSymbolExtensions
{
    internal static bool MatchesType(this INamedTypeSymbol symbol, string typeName)
    {
        if (symbol == null)
            return false;

        var symbolTypeAsString = symbol.ToString().Replace("?", "");

        if (symbol.IsGenericType)
            return symbolTypeAsString.StartsWith(($"{typeName}<"));
        else
            return symbolTypeAsString == typeName;
    }
}
