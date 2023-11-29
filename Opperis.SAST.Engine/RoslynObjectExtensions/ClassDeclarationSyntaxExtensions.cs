using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class ClassDeclarationSyntaxExtensions
    {
        //TODO: Change parentClassName to a param string[] to avoid multiple inheritance crawls when checking for multiple classes
        internal static bool InheritsFrom(this ClassDeclarationSyntax syntax, string parentClassName)
        {
            var model = Globals.SearchForSemanticModel(syntax.SyntaxTree); //Globals.Compilation.GetSemanticModel(syntax.SyntaxTree);
            var symbol = model.GetDeclaredSymbol(syntax) as INamedTypeSymbol;

            INamedTypeSymbol parentType = model.Compilation.GetTypeByMetadataName(parentClassName);

            if (symbol.Equals(parentType, SymbolEqualityComparer.Default))
                return true;

            var baseType = symbol.BaseType;

            while (baseType != null)
            {
                if (baseType.Equals(parentType, SymbolEqualityComparer.Default))
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
        }

        internal static bool IsTestClass(this ClassDeclarationSyntax syntax)
        {
            var model = Globals.SearchForSemanticModel(syntax.SyntaxTree);

            foreach (var list in syntax.AttributeLists)
            {
                foreach (var attribute in list.Attributes)
                {
                    var attributeType = model.GetTypeInfo(attribute).Type.ToString();

                    if (attributeType.StartsWith("Xunit."))
                        return true;
                    else if (attributeType.StartsWith("Microsoft.VisualStudio.TestTools.UnitTesting."))
                        return true;
                }
            }

            return false;
        }
    }
}
