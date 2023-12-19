using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class SymmetricCryptographyPropertySyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<MemberAccessExpressionSyntax> CryptoKeySets { get; private set; } = new List<MemberAccessExpressionSyntax>();
    public List<MemberAccessExpressionSyntax> CryptoIVSets { get; private set; } = new List<MemberAccessExpressionSyntax>();
    public List<MemberAccessExpressionSyntax> CryptoModeSets { get; private set; } = new List<MemberAccessExpressionSyntax>();

    public bool HasRun => CryptoKeySets.Any() || CryptoIVSets.Any() || CryptoModeSets.Any();

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        CheckForKeySets(node);
        base.VisitMethodDeclaration(node);
    }

    private void CheckForKeySets(SyntaxNode node)
    {
        foreach (var child in node.DescendantNodes())
        {
            var member = child as MemberAccessExpressionSyntax;

            if (member != null && IsDangerousCryptoProperty(member))
            {
                var parent = member.Parent;

                while (parent != null)
                {
                    if (parent is AssignmentExpressionSyntax assignment)
                    {
                        if (member.Name.Identifier.Text == "Key")
                            CryptoKeySets.Add(member);
                        else if (member.Name.Identifier.Text == "IV")
                            CryptoIVSets.Add(member);
                        else if (member.Name.Identifier.Text == "Mode")
                            CryptoModeSets.Add(member);

                        break;
                    }
                    else
                    {
                        parent = parent.Parent;
                    }
                }
            }
        }
    }

    private bool IsDangerousCryptoProperty(MemberAccessExpressionSyntax memberAccess)
    {
        if (memberAccess.Name.Identifier.Text != "Key" && memberAccess.Name.Identifier.Text != "IV" && memberAccess.Name.Identifier.Text != "Mode")
            return false;

        var identifierName = memberAccess.Expression as IdentifierNameSyntax;

        if (identifierName == null)
            return false;

        var semanticModel = Globals.Compilation.GetSemanticModel(identifierName.SyntaxTree);

        var symbol = semanticModel.GetSymbolInfo(identifierName).Symbol as ILocalSymbol;
        if (symbol == null)
            return false;

        var type = symbol.Type;

        while (type != null)
        {
            if (type.ToString() == "System.Security.Cryptography.SymmetricAlgorithm")
                return true;
            else
                type = type.BaseType;
        }

        return false;
    }
}
