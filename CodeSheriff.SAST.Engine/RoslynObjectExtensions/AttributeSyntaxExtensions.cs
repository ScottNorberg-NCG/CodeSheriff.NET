using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.RoslynObjectExtensions;

internal static class AttributeSyntaxExtensions
{
    internal static bool IsOfType(this AttributeSyntax attribute, string typeName, SemanticModel model)
    {
        return model.GetTypeInfo(attribute).Type.ToString().Replace("?", "") == typeName;
    }

    internal static bool HasCsrfAttribute(this AttributeSyntax attributeSyntax, SemanticModel model)
    {
        return model.GetTypeInfo(attributeSyntax).Type.ToString().Replace("?", "").In("Microsoft.AspNetCore.Mvc.ValidateAntiForgeryTokenAttribute",
                                                                     "System.Web.Mvc.ValidateAntiForgeryTokenAttribute",
                                                                     "Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute",
                                                                     "System.Web.Mvc.AutoValidateAntiforgeryTokenAttribute");
    }

    internal static bool HasBindingSourceAttribute(this AttributeSyntax attributeSyntax, SemanticModel model)
    {
        var asSymbol = model.GetSymbolInfo(attributeSyntax).Symbol;

        if (asSymbol != null)
            return asSymbol.ContainingType.AllInterfaces.Any(i => i.Name == "IBindingSourceMetadata");

        return false;
    }

    internal static bool IsTestAttribute(this AttributeSyntax attributeSyntax, SemanticModel model)
    {
        var typeName = model.GetTypeInfo(attributeSyntax).Type.ToString();

        if (typeName == "SkippableTheory") //TODO: Is there a way we can get the fact that this is actually an Xunit attribute?
            return true;
        else if (typeName == "Fact")
            return true;
        else if (typeName.StartsWith("Xunit."))
            return true;
        else if (typeName.StartsWith("Microsoft.VisualStudio.TestTools.UnitTesting."))
            return true;

        return false;
    }
}
