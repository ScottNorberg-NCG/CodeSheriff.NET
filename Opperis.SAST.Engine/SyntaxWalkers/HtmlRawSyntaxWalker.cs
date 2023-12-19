using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class HtmlRawSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    internal List<InvocationExpressionSyntax> HtmlRawCalls = new List<InvocationExpressionSyntax>();

    public bool HasRun => HtmlRawCalls.Any();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (node.Expression is MemberAccessExpressionSyntax memberAccess &&
            memberAccess.Name.Identifier.Text == "Raw")
        {
            var symbol = memberAccess.ToSymbol();

            if (symbol != null && symbol.ContainingType.Name == "IHtmlHelper")
            {
                HtmlRawCalls.Add(node);
            }
        }

        base.VisitInvocationExpression(node);
    }
}
