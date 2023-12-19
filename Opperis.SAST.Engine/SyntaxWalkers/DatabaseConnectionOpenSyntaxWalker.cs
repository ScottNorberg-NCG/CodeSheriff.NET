using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class DatabaseConnectionOpenSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    internal List<InvocationExpressionSyntax> ConnectionOpens = new List<InvocationExpressionSyntax>();

    public bool HasRun => ConnectionOpens.Any();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (node.Expression is MemberAccessExpressionSyntax memberAccess &&
            memberAccess.Name.Identifier.Text == "Open")
        {
            var model = Globals.Compilation.GetSemanticModel(node.SyntaxTree);
            var symbol = model.GetSymbolInfo(memberAccess).Symbol;

            if (symbol == null)
            {
                Globals.RuntimeErrors.Add(new NoSymbolForExpression(memberAccess));
            }
            else if (symbol.ContainingType.Name == "SqlConnection")
            {
                if (!node.Ancestors().OfType<UsingStatementSyntax>().Any())
                    ConnectionOpens.Add(node);
            }
        }

        base.VisitInvocationExpression(node);
    }
}
