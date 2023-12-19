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

internal class UIProcessorMethodSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<MethodDeclarationSyntax> ControllerMethods { get; private set; } = new List<MethodDeclarationSyntax>();
    public List<MethodDeclarationSyntax> RazorPageMethods { get; private set; } = new List<MethodDeclarationSyntax>();

    public bool HasRun => ControllerMethods.Any() || RazorPageMethods.Any();

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.IsControllerMethod())
            ControllerMethods.Add(node);
        else if (node.IsRazorMethod())
            RazorPageMethods.Add(node);
    }
}
