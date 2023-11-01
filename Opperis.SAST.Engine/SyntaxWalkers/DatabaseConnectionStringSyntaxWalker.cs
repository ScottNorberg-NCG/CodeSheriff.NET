using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers
{
    internal class DatabaseConnectionStringSyntaxWalker : CSharpSyntaxWalker
    {
        public List<MemberAccessExpressionSyntax> ConnectionStringSets { get; private set; } = new List<MemberAccessExpressionSyntax>();
        public List<ObjectCreationExpressionSyntax> NewConnectionStrings { get; private set; } = new List<ObjectCreationExpressionSyntax>();

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
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
            if (memberAccess.Expression is IdentifierNameSyntax identifierName)
            {
                var objectType = identifierName.GetUnderlyingType();

                if (objectType != null)
                {
                    var typeString = objectType.ToString();

                    if (typeString == "Microsoft.Data.SqlClient.SqlConnection" || typeString == "System.Data.SqlClient.SqlConnection")
                    {
                        return memberAccess.Name.Identifier.Text == "ConnectionString";
                    }
                }
            }

            return false;
        }
    }
}
