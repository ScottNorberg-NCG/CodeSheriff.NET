using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class EntityFrameworkDbCallSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    internal List<InvocationExpressionSyntax> DatabaseCalls = new List<InvocationExpressionSyntax>();

    public bool HasRun => DatabaseCalls.Any();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (node.Expression is MemberAccessExpressionSyntax dbSetMember &&
            dbSetMember.Name.Identifier.Text.In("FromSqlRaw"))
        {
            var type = dbSetMember.Expression.GetUnderlyingType();

            if (type != null)
            {
                var typeString = type.ToString();

                if (typeString.IndexOf("<") > -1)
                {
                    if (typeString.IndexOf("<") > -1)
                    {
                        var genericType = typeString.Substring(0, typeString.IndexOf("<"));

                        if (genericType == "Microsoft.EntityFrameworkCore.DbSet")
                            DatabaseCalls.Add(node);
                    }
                }
            } 
        }

        if (node.Expression is MemberAccessExpressionSyntax databaseMember &&
            databaseMember.Name.Identifier.Text.In("ExecuteSqlRaw", "ExecuteSqlRawAsync"))
        {
            if (node.IsInvocationFromType("Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade"))
                DatabaseCalls.Add(node);
        }

        base.VisitInvocationExpression(node);
    }
}
