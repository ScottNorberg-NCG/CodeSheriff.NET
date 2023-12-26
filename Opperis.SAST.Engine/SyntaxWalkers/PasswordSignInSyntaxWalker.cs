using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class PasswordSignInSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    internal List<InvocationExpressionSyntax> SignIns = new List<InvocationExpressionSyntax>();

    public bool HasRun => SignIns.Any();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var asSymbol = node.ToSymbol();

        if (asSymbol != null && asSymbol.ContainingType != null) 
        {
            if (asSymbol.ContainingType.ToString().Replace("?", "").StartsWith("Microsoft.AspNetCore.Identity.SignInManager<"))
            {
                if (node.Expression is MemberAccessExpressionSyntax member)
                {
                    if (member.Name.Identifier.Text == "PasswordSignInAsync")
                    {
                        SignIns.Add(node);
                    }
                }
            }
        }

        //base.VisitInvocationExpression(node);
    }
}
