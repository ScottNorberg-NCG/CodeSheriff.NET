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

internal class DatabaseCommandTextSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<MemberAccessExpressionSyntax> CommandTextSets { get; private set; } = new List<MemberAccessExpressionSyntax>();

    public bool HasRun => CommandTextSets.Any();

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        CheckForCommandTextSets(node);
        base.VisitMethodDeclaration(node);
    }

    private void CheckForCommandTextSets(SyntaxNode node)
    {
        foreach (var child in node.DescendantNodes())
        {
            var member = child as MemberAccessExpressionSyntax;

            if (member != null && IsDatabaseConnectionMethod(member))
            {
                CommandTextSets.Add(member);
            }
        }
    }

    private bool IsDatabaseConnectionMethod(MemberAccessExpressionSyntax memberAccess)
    {
        if (memberAccess.Name.Identifier.Text != "CommandText")
            return false;

        var identifierName = memberAccess.Expression as IdentifierNameSyntax;

        if (identifierName == null)
            return false;

        var objectType = identifierName.GetUnderlyingType();

        if (objectType != null)
        { 
            var typeString = objectType.ToString().Replace("?", "");

            if (typeString.In("Microsoft.Data.SqlClient.SqlCommand", "System.Data.SqlClient.SqlCommand", "System.Data.Common.DbCommand", 
                "Microsoft.Data.Sqlite.SqliteCommand", "System.Data.OleDb.OleDbCommand", "System.Data.Odbc.OdbcCommand", "IBM.Data.DB2.DB2Command"))
            {
                return true;
            }            
        }

        return false;
    }
}
