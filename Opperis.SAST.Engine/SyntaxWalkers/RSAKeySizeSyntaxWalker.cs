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

internal class RSAKeySizeSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<MemberAccessExpressionSyntax> KeyLengthSets { get; private set; } = new List<MemberAccessExpressionSyntax>();

    public bool HasRun => KeyLengthSets.Any();

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        CheckForKeySizeSets(node);
        base.VisitMethodDeclaration(node);
    }

    private void CheckForKeySizeSets(SyntaxNode node)
    {
        foreach (var child in node.DescendantNodes())
        {
            var member = child as MemberAccessExpressionSyntax;

            if (member != null && IsKeySizeSetProperty(member))
            {
                KeyLengthSets.Add(member);
            }
        }
    }

    private bool IsKeySizeSetProperty(MemberAccessExpressionSyntax memberAccess)
    {
        if (memberAccess.Name.Identifier.Text != "KeySize")
            return false;

        if (memberAccess.Expression is IdentifierNameSyntax identifierName)
        {
            var objectType = identifierName.GetUnderlyingType();

            if (objectType != null)
            {
                var typeString = objectType.ToString().Replace("?", "");

                if (typeString == "System.Security.Cryptography.RSACryptoServiceProvider")
                {
                    return true;
                }
            }
        }

        return false;
    }
}
