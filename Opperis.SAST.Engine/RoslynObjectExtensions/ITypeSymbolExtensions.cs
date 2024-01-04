using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions;

internal static class ITypeSymbolExtensions
{
    internal static List<PropertyDeclarationSyntax> GetProperties(this ITypeSymbol symbol)
    {
        var asNode = symbol.DeclaringSyntaxReferences.First().GetSyntax();

        if (asNode is ClassDeclarationSyntax asClass)
            return asClass.Members.OfType<PropertyDeclarationSyntax>().ToList();
        else
            return null;
    }

    internal static bool IsEntityFrameworkType(this ITypeSymbol symbol)
    {
        foreach (var type in Globals.EntityFrameworkObjects)
        {
            if (type.Equals(symbol))
                return true;
        }

        return false;
    }
}
