using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.RoslynObjectExtensions;

internal static class LiteralExpressionSyntaxExtensions
{
    internal static T GetLiteralValue<T>(this LiteralExpressionSyntax literal)
    { 
        if (literal != null)
            return (T)Convert.ChangeType(literal.Token.Value, typeof(T));
        else
            return default;
    }
}
