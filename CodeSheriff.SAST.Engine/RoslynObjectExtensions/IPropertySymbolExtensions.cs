using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.RoslynObjectExtensions;

internal static class IPropertySymbolExtensions
{
    internal static bool HasValidatorAttribute(this IPropertySymbol symbol) 
    {
        return symbol.GetAttributes().Any(a => a.InheritsFrom("System.ComponentModel.DataAnnotations.ValidationAttribute"));
    }
}
