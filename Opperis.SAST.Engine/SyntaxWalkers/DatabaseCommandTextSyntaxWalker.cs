﻿using Microsoft.CodeAnalysis;
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
    internal class DatabaseCommandTextSyntaxWalker : CSharpSyntaxWalker
    {
        public List<MemberAccessExpressionSyntax> CommandTextSets { get; private set; } = new List<MemberAccessExpressionSyntax>();

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
            var identifierName = memberAccess.Expression as IdentifierNameSyntax;

            if (identifierName == null)
                return false;

            var objectType = identifierName.GetUnderlyingType();

            if (objectType != null)
            { 
                var typeString = objectType.ToString();

                if (typeString == "Microsoft.Data.SqlClient.SqlCommand" || typeString == "System.Data.SqlClient.SqlCommand" || typeString == "System.Data.Common.DbCommand")
                {
                    return memberAccess.Name.Identifier.Text == "CommandText";
                }            
            }

            return false;
        }
    }
}