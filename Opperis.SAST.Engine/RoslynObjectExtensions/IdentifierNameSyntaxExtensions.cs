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
    internal static List<IdentifierNameSyntax> GetReferences(this IdentifierNameSyntax id)
    {
        var parentBlock = id.Ancestors().FirstOrDefault(a => a is BlockSyntax) as BlockSyntax;

        if (parentBlock == null)
            return new List<IdentifierNameSyntax>();

        //We're in the same block, so we should not have different variables of the same name
        return parentBlock.DescendantNodes().OfType<IdentifierNameSyntax>().Where(n => n.Identifier.Text == id.Identifier.Text).ToList();
    }

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

    internal static bool IsAssignmentSourceInTree(this IdentifierNameSyntax id)
    {
        var assignment = id.Ancestors().FirstOrDefault(a => a is AssignmentExpressionSyntax) as AssignmentExpressionSyntax;

        if (assignment == null)
            return false;

        if (assignment.Right is MemberAccessExpressionSyntax memberAccess)
            return memberAccess.Expression.IsIncrementallyIdenticalTo(id);
        else
            return false;
    }

    internal static bool IsAssignmentDestinationInTree(this IdentifierNameSyntax id)
    {
        var assignment = id.Ancestors().FirstOrDefault(a => a is AssignmentExpressionSyntax) as AssignmentExpressionSyntax;

        if (assignment == null)
            return false;

        if (assignment.Left is MemberAccessExpressionSyntax memberAccess)
            return memberAccess.Expression.IsIncrementallyIdenticalTo(id);
        else
            return false;
    }
}
