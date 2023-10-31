using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers
{
    internal class ExternalRedirectSyntaxWalker : CSharpSyntaxWalker
    {
        internal List<InvocationExpressionSyntax> UnvalidatedRedirects = new List<InvocationExpressionSyntax>();

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is IdentifierNameSyntax id && id.Identifier.Text == "Redirect")
            {
                var model = Globals.Compilation.GetSemanticModel(node.SyntaxTree);
                var symbol = model.GetSymbolInfo(id).Symbol;

                if (symbol.ContainingType.Name == "PageModel" || symbol.ContainingType.Name == "ControllerBase")
                {
                    UnvalidatedRedirects.Add(node);
                }
            }

            base.VisitInvocationExpression(node);
        }
    }
}
