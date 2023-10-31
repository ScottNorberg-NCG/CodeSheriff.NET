using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class AttributeSyntaxExtensions
    {
        internal static bool HasCsrfAttribute(this AttributeSyntax attributeSyntax, SemanticModel model)
        {
            return model.GetTypeInfo(attributeSyntax).Type.ToString().In("Microsoft.AspNetCore.Mvc.ValidateAntiForgeryTokenAttribute",
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
    }
}
