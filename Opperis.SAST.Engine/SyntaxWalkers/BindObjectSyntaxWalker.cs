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

internal class BindObjectSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    internal List<BindObjectInfo> BindObjectReferences { get; private set; } = new List<BindObjectInfo>();

    public bool HasRun => BindObjectReferences.Any();

    public override void VisitIdentifierName(IdentifierNameSyntax node)
    {
        var parentClass = node.FirstAncestorOrSelf<ClassDeclarationSyntax>();

        if (parentClass != null)
        {
            if (node.GetDefinitionNode(parentClass) is PropertyDeclarationSyntax prop)
            {
                var model = Globals.Compilation.GetSemanticModel(parentClass.SyntaxTree);
                if (!prop.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.IsOfType("Microsoft.AspNetCore.Mvc.BindPropertyAttribute", model)))
                    return;

                if (node.GetUnderlyingType() is INamedTypeSymbol type)
                {
                    if (type.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is ClassDeclarationSyntax classDeclaration)
                    {
                        if (!BindObjectReferences.Any(o => o.AsClass == classDeclaration))
                            BindObjectReferences.Add(new BindObjectInfo() { AsClass = classDeclaration, AsType = type });
                    }
                }
            }
        }

        base.VisitIdentifierName(node);
    }

    //public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    //{
    //    if (node.Expression != null && node.Expression is IdentifierNameSyntax id)
    //    {
    //        var parent = node.FirstAncestorOrSelf<ClassDeclarationSyntax>();

    //        if (parent != null)
    //        {
    //            if (id.GetDefinitionNode(parent) is PropertyDeclarationSyntax prop)
    //            {
    //                var model = Globals.Compilation.GetSemanticModel(parent.SyntaxTree);
    //                if (!prop.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.IsOfType("Microsoft.AspNetCore.Mvc.BindPropertyAttribute", model)))
    //                    return;

    //                if (id.GetUnderlyingType() is INamedTypeSymbol type)
    //                {
    //                    if (type.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is ClassDeclarationSyntax classDeclaration)
    //                    {
    //                        if (!BindObjectReferences.Any(o => o.AsClass == classDeclaration))
    //                            BindObjectReferences.Add(new BindObjectInfo() { AsClass = classDeclaration, AsType = type });
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public struct BindObjectInfo
    { 
        public ClassDeclarationSyntax AsClass { get; set; }
        public INamedTypeSymbol AsType { get; set; }
    }
}
