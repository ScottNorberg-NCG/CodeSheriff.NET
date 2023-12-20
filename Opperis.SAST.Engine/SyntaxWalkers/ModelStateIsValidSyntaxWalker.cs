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

internal class ModelStateIsValidSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<MemberAccessExpressionSyntax> ModelStateIsValidChecks { get; private set; } = new List<MemberAccessExpressionSyntax>();

    public bool HasRun => ModelStateIsValidChecks.Any();

    public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        if (node.Name.Identifier.Text != "IsValid")
            return;

        var identifierName = node.Expression as IdentifierNameSyntax;

        if (identifierName == null)
            return;

        var objectType = identifierName.GetUnderlyingType();

        if (objectType != null)
        {
            var typeString = objectType.ToString().Replace("?", "");

            if (typeString == "Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary")
            {
                ModelStateIsValidChecks.Add(node);
            }
        }
    }
}
