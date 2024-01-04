using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions;

internal static class InterpolatedStringExpressionSyntaxExtensions
{
    internal static List<ExpressionSyntax> GetNonLiteralPortions(this InterpolatedStringExpressionSyntax interpolated)
    {
        List<ExpressionSyntax> inputs = new List<ExpressionSyntax>();

        foreach (var content in interpolated.Contents)
        {
            if (content is InterpolationSyntax interpolation)
            {
                inputs.Add(interpolation.Expression);
            }
        }

        return inputs;
    }
}
