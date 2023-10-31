using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class MethodDeclarationSyntaxExtensions
    {
        internal static bool IsUIProcessor(this MethodDeclarationSyntax method)
        {
            if (!method.Modifiers.Any(m => m.ValueText == "public"))
                return false;

            if (method.Parent is ClassDeclarationSyntax classDeclaration)
            {
                var model = Globals.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;
                INamedTypeSymbol pageType = model.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.RazorPages.PageModel");
                INamedTypeSymbol controllerType = model.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.Controller");

                var baseType = symbol.BaseType;

                while (baseType != null)
                {
                    if (baseType.Equals(pageType, SymbolEqualityComparer.Default)) 
                    {
                        var methodName = method.Identifier.Text;

                        if (methodName == "OnPostAsync" || methodName == "OnGetAsync")
                            return true;
                    }
                    else if (baseType.Equals(controllerType, SymbolEqualityComparer.Default)) 
                    {
                        //TODO: look at return type too to be sure
                        return true;
                    }

                    baseType = baseType.BaseType;
                }
            }

            return false;
        }
    }
}
