using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.RoslynObjectExtensions
{
    internal static class SyntaxTokenExtensions
    {
        internal static ExpressionSyntax ToExpressionSyntax(this SyntaxToken token) 
        {
            var root = token.SyntaxTree.GetRoot();
            return root.FindNode(token.Span) as ExpressionSyntax;
        }
    }
}
