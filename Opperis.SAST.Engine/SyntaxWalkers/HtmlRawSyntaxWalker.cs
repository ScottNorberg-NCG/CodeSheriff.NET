using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers
{
    internal class HtmlRawSyntaxWalker : CSharpSyntaxWalker
    {
        internal List<InvocationExpressionSyntax> HtmlRawCalls = new List<InvocationExpressionSyntax>();

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.Text == "Raw")
            {
                var model = Globals.Compilation.GetSemanticModel(node.SyntaxTree);
                var symbol = model.GetSymbolInfo(memberAccess).Symbol;

                if (symbol != null && symbol.ContainingType.Name == "IHtmlHelper")
                {
                    HtmlRawCalls.Add(node);
                }
            }

            base.VisitInvocationExpression(node);
        }
    }
}
