using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers
{
    internal class DatabaseConnectionCloseSyntaxWalker : CSharpSyntaxWalker
    {
        internal List<InvocationExpressionSyntax> ConnectionCloses = new List<InvocationExpressionSyntax>();

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.Text == "Close")
            {
                var model = Globals.Compilation.GetSemanticModel(node.SyntaxTree);
                var symbol = model.GetSymbolInfo(memberAccess).Symbol;

                if (symbol.ContainingType.Name == "SqlConnection")
                {
                    ConnectionCloses.Add(node);
                }
            }

            base.VisitInvocationExpression(node);
        }
    }
}
