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
    internal class RazorPageBindObjectSyntaxWalker : CSharpSyntaxWalker
    {
        public List<RazorPageBindObject> RazorPageBindObjects { get; } = new List<RazorPageBindObject>();

        public override void VisitAttribute(AttributeSyntax node)
        {
            if (node.Name != null && node.Name.ToSymbol() != null)
            {
                var type = node.Name.ToSymbol().ContainingType;

                if (type.ToDisplayString() == "Microsoft.AspNetCore.Mvc.BindPropertyAttribute")
                {
                    if (node.Parent.Parent is PropertyDeclarationSyntax property)
                    {
                        var parent = property.Parent;

                        while (parent != null) 
                        { 
                            if (parent is ClassDeclarationSyntax classDeclaration) 
                            {
                                RazorPageBindObjects.Add(new RazorPageBindObject() { ClassDeclaration = classDeclaration, ObjectType = property.Type.GetUnderlyingType() });
                            }

                            parent = parent.Parent;
                        }
                    }
                }
            }

            base.VisitAttribute(node);
        }

        internal struct RazorPageBindObject
        { 
            public ITypeSymbol ObjectType { get; set; }
            public ClassDeclarationSyntax ClassDeclaration { get; set; }
        }
    }
}
