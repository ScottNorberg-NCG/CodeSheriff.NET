using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions;

internal static class BinaryExpressionSyntaxExtensions
{
    internal static List<ExpressionSyntax> GetNonLiteralPortions(this BinaryExpressionSyntax binaryExpression)
    {
        List<ExpressionSyntax> inputs = new List<ExpressionSyntax>();

        GetNonLiteralPortions(binaryExpression, inputs);

        return inputs;
    }

    private static void GetNonLiteralPortions(BinaryExpressionSyntax binaryExpression, List<ExpressionSyntax> list)
    {
        if (binaryExpression.Left is BinaryExpressionSyntax left)
            GetNonLiteralPortions(left, list);
        else if (!(binaryExpression.Left is LiteralExpressionSyntax))
            list.Add(binaryExpression.Left);

        if (binaryExpression.Right is BinaryExpressionSyntax right)
            GetNonLiteralPortions(right, list);
        else if (!(binaryExpression.Right is LiteralExpressionSyntax))
            list.Add(binaryExpression.Right);
    }
}
