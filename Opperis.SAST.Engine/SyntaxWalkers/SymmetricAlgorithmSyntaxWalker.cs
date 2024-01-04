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

internal class SymmetricAlgorithmSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    internal List<MemberAccessExpressionSyntax> SymmetricAlgorithms { get; private set; } = new List<MemberAccessExpressionSyntax>();

    public bool HasRun => SymmetricAlgorithms.Any();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var asString = node.Expression.ToString();

        if (asString == "CreateEncryptor" || asString.EndsWith(".CreateEncryptor") || asString == "CreateDecryptor" || asString.EndsWith(".CreateDecryptor"))
        {
            var encryptionType = GetEncryptionAlgorithmType(node);
            if (encryptionType != null)
            {
                var memberSyntax = node.Expression as MemberAccessExpressionSyntax;
                SymmetricAlgorithms.Add(memberSyntax);
            }
        }

        base.VisitInvocationExpression(node);
    }

    private static ITypeSymbol? GetEncryptionAlgorithmType(InvocationExpressionSyntax syntax)
    {
        var memberSyntax = syntax.Expression as MemberAccessExpressionSyntax;

        if (memberSyntax == null)
            return null;

        var identifierName = memberSyntax.Expression as IdentifierNameSyntax;

        if (identifierName == null)
            return null;

        //var semanticModel = Globals.Compilation.GetSemanticModel(identifierName.SyntaxTree);

        //var symbol = semanticModel.GetSymbolInfo(identifierName).Symbol as ILocalSymbol;
        //if (symbol == null)
        //    return null;

        var type = identifierName.GetUnderlyingType();

        while (type != null)
        {
            if (type.ToString().Replace("?", "") == "System.Security.Cryptography.SymmetricAlgorithm")
                return identifierName.GetUnderlyingType(); //symbol.Type;
            else
                type = type.BaseType;
        }

        return null;
    }
}
