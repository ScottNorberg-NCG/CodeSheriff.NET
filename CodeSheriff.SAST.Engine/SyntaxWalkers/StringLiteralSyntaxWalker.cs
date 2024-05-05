using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal class StringLiteralSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<LiteralExpressionSyntax> StringLiterals { get; private set; } = new List<LiteralExpressionSyntax>();

    public bool HasRun => StringLiterals.Any();

    public override void VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        if (node.Kind().ToString() == "StringLiteralExpression")
            StringLiterals.Add(node);
    }
}
