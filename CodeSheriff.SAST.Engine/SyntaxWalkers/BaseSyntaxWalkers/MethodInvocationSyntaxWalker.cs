using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers.BaseSyntaxWalkers
{
    internal abstract class MethodInvocationSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
    {
        protected abstract List<string> MethodNames { get; }
        protected abstract List<string> ObjectContainerNames { get; }

        internal List<InvocationExpressionSyntax> MethodCalls = new List<InvocationExpressionSyntax>();

        public bool HasRun => MethodCalls.Any();

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.Text.In(MethodNames))
            {
                var symbol = memberAccess.ToSymbol() as IMethodSymbol;

                if (symbol != null && ObjectContainerNames.Any(c => symbol.ContainingType.MatchesType(c)))
                {
                    MethodCalls.Add(node);
                }
            }

            base.VisitInvocationExpression(node);
        }
    }
}
