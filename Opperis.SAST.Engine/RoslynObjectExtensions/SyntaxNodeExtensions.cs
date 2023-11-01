using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class SyntaxNodeExtensions
    {
        internal static string GetDisplayText(this SyntaxNode node)
        {
            if (node is NamespaceDeclarationSyntax namespaceDeclaration)
                return namespaceDeclaration.Name.ToString();
            else if (node is ClassDeclarationSyntax classDeclaration)
                return classDeclaration.Identifier.Text;
            else if (node is MethodDeclarationSyntax methodDeclaration)
                return methodDeclaration.Identifier.Text;
            else if (node is VariableDeclaratorSyntax variable)
                return variable.ToString();
            else if (node is ParameterSyntax parameter)
                return parameter.ToString();
            else if (node is StructDeclarationSyntax structSyntax)
                return structSyntax.Identifier.Text;
            else if (node is PropertyDeclarationSyntax prop)
                return prop.Identifier.Text;
            else
                throw new NotImplementedException($"Cannot find display text for type: {node.GetType()}");
        }
    }
}
