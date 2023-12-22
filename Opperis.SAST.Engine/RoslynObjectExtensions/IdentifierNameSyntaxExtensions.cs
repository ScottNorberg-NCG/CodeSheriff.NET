using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.Findings.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions;

internal static class IdentifierNameSyntaxExtensions
{
    internal static T GetLiteralValue<T>(this IdentifierNameSyntax id)
    {
        var definition = id.GetDefinitionNode(id.Ancestors().Last());

        if (definition != null)
        {
            //Check to see if we have two nodes. If so, this should be an EqualsValueClauseSyntax and a LiteralExpressionSyntax
            if (definition.DescendantNodes().Count() == 2)
            {
                var literal = definition.DescendantNodes().SingleOrDefault(n => n is LiteralExpressionSyntax) as LiteralExpressionSyntax;
                return literal.GetLiteralValue<T>();
            }
            else
                return default;
        }
        else
            return default;
    }
}
