using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers.BaseSyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal class EntityFrameworkSaveSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    internal List<MemberAccessExpressionSyntax> MethodCalls { get; private set; } = new List<MemberAccessExpressionSyntax>();

    public bool HasRun => MethodCalls.Any();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (node.Expression is MemberAccessExpressionSyntax member)
        {
            var asString = member.Name.ToString();

            if (asString == "SaveChanges" || asString == "SaveChangesAsync")
            {
                if (IsDatabaseContext(node))
                {
                    var memberSyntax = node.Expression as MemberAccessExpressionSyntax;
                    MethodCalls.Add(memberSyntax);
                }
            }
        }

        base.VisitInvocationExpression(node);
    }

    private static bool IsDatabaseContext(InvocationExpressionSyntax syntax)
    {
        var memberSyntax = syntax.Expression as MemberAccessExpressionSyntax;

        if (memberSyntax == null)
            return false;

        var identifierName = memberSyntax.Expression as IdentifierNameSyntax;

        if (identifierName == null)
            return false;

        var type = identifierName.GetUnderlyingType();

        while (type != null)
        {
            if (type.ToString().Replace("?", "") == "Microsoft.EntityFrameworkCore.DbContext")
                return true;
            else
                type = type.BaseType;
        }

        return false;
    }
}
