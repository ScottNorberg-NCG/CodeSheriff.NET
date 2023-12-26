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

internal class IUserStoreSyntaxWalker : CSharpSyntaxWalker
{
    public List<INamedTypeSymbol> IUserStoreClasses = new List<INamedTypeSymbol>();

    public bool HasRun => IUserStoreClasses.Any();

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var model = Globals.SearchForSemanticModel(node.SyntaxTree);
        var symbol = model.GetDeclaredSymbol(node);

        if (symbol.Interfaces.Any(i => i.ToString().StartsWith("Microsoft.AspNetCore.Identity.IUserStore<")))
            IUserStoreClasses.Add(symbol);

        base.VisitClassDeclaration(node);
    }
}
