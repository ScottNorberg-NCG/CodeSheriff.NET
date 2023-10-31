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
    internal class ControllerMethodSyntaxWalker : CSharpSyntaxWalker
    {
        public List<MethodDeclarationSyntax> Methods { get; private set; } = new List<MethodDeclarationSyntax>();

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.InheritsFrom("Microsoft.AspNetCore.Mvc.Controller") || node.InheritsFrom("System.Web.Mvc.Controller"))
                base.VisitClassDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword)))
            {
                Methods.Add(node);
            }
        }
    }
}
