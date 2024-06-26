﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal class EntityFrameworkDbSetSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<ITypeSymbol> EntityFrameworkObjects { get; } = new List<ITypeSymbol>();

    public bool HasRun => EntityFrameworkObjects.Any();

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        if (node.InheritsFrom("Microsoft.EntityFrameworkCore.DbContext"))
        {
            if (!node.IsTestClass())
                base.VisitClassDeclaration(node);
        }
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        var asGenericType = node.Type as GenericNameSyntax;

        if (node.Type.GetUnderlyingType() != null)
        {
            if (node.Type.GetUnderlyingType().ToString().StartsWith("Microsoft.EntityFrameworkCore.DbSet<"))
            {
                var objectType = asGenericType.TypeArgumentList.Arguments.First().GetUnderlyingType();
                EntityFrameworkObjects.Add(objectType);
            }
        }

        base.VisitPropertyDeclaration(node);
    }
}
