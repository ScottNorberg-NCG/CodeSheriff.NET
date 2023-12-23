using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class ComputeHashSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    internal List<InvocationExpressionSyntax> ComputeHashCalls = new List<InvocationExpressionSyntax>();

    public bool HasRun => ComputeHashCalls.Any();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (node.Expression is MemberAccessExpressionSyntax memberAccess &&
            memberAccess.Name.Identifier.Text == "ComputeHash")
        {
            var symbol = memberAccess.ToSymbol();

            if (symbol != null && symbol.ContainingType.ToString() == "System.Security.Cryptography.HashAlgorithm")
            {
                ComputeHashCalls.Add(node);
            }
        }

        base.VisitInvocationExpression(node);
    }
}
