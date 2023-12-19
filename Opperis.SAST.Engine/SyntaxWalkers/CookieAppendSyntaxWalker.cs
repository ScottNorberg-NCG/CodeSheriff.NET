using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class CookieAppendSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<InvocationExpressionSyntax> CookieAdds = new List<InvocationExpressionSyntax>();

    public bool HasRun => CookieAdds.Any();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (node.Expression is MemberAccessExpressionSyntax member)
        {
            if (member.Name.ToString() == "Append")
            {
                var asSymbol = node.ToSymbol();

                if (asSymbol is IMethodSymbol method)
                {
                    if (method.ContainingType != null)
                    {
                        if (method.ContainingType.ToString() == "Microsoft.AspNetCore.Http.IResponseCookies")
                            CookieAdds.Add(node);
                    }
                }
            }
        }

        base.VisitInvocationExpression(node);
    }
}
