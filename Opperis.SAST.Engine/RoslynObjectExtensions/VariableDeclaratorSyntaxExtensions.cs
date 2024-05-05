using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.RoslynObjectExtensions
{
    internal static class VariableDeclaratorSyntaxExtensions
    {
        internal static List<IdentifierNameSyntax> GetReferences(this VariableDeclaratorSyntax id)
        {
            var parentBlock = id.Ancestors().FirstOrDefault(a => a is BlockSyntax) as BlockSyntax;

            if (parentBlock == null)
                return new List<IdentifierNameSyntax>();

            //We're in the same block, so we should not have different variables of the same name
            return parentBlock.DescendantNodes().OfType<IdentifierNameSyntax>().Where(n => n.Identifier.Text == id.Identifier.Text).ToList();
        }
    }
}
