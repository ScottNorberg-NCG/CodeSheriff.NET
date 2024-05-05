using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal class DatabaseConnectionStringSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<MemberAccessExpressionSyntax> ConnectionStringSets { get; private set; } = new List<MemberAccessExpressionSyntax>();
    public List<ObjectCreationExpressionSyntax> NewConnectionStrings { get; private set; } = new List<ObjectCreationExpressionSyntax>();

    public bool HasRun => ConnectionStringSets.Any() || NewConnectionStrings.Any();

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.Parent is ClassDeclarationSyntax classDeclaration &&
            classDeclaration.IsTestClass())
        {
            base.VisitMethodDeclaration(node);
            return;
        }
        
        foreach (var child in node.DescendantNodes())
        {
            var member = child as MemberAccessExpressionSyntax;

            if (member != null && IsDatabaseConnectionSet(member))
            {
                ConnectionStringSets.Add(member);
            }
        }

        base.VisitMethodDeclaration(node);
    }

    public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        if (node.Type is IdentifierNameSyntax identifier)
        {
            if (identifier.Identifier.Text == "SqlConnection")
            {
                NewConnectionStrings.Add(node);
            }
        }

        base.VisitObjectCreationExpression(node);
    }

    private bool IsDatabaseConnectionSet(MemberAccessExpressionSyntax memberAccess)
    {
        if (memberAccess.Name.Identifier.Text != "ConnectionString")
            return false;

        if (memberAccess.Expression is IdentifierNameSyntax identifierName)
        {
            var objectType = identifierName.GetUnderlyingType();

            if (objectType != null)
            {
                var typeString = objectType.ToString();

                if (typeString.Replace("?", "").In("Microsoft.Data.SqlClient.SqlConnection", "System.Data.SqlClient.SqlConnection"))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
