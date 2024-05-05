using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal class HtmlHelperSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<MethodDeclarationSyntax> UnsafeHtmlHelpers = new List<MethodDeclarationSyntax>();

    public bool HasRun => UnsafeHtmlHelpers.Any();

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.IsExtensionMethod())
        {
            if (node.ReturnType.GetUnderlyingType() != null && //Likely due to return type of "void"
                node.ReturnType.GetUnderlyingType().ToDisplayString() == "Microsoft.AspNetCore.Html.IHtmlContent" && 
                node.ParameterList.Parameters.First().Type.GetUnderlyingType().ToDisplayString() == "Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper")
            {
                bool hasEncoder = false;

                foreach (var child in node.DescendantNodes().Where(c => c is InvocationExpressionSyntax))
                {
                    var invocation = child as InvocationExpressionSyntax;

                    if (invocation.Expression is MemberAccessExpressionSyntax member)
                    {
                        //TODO: Look at the full namespace
                        //TODO: Look at the encoder to make sure it's being used in the right places
                        //System.Web.HttpUtility.HtmlEncode
                        //System.Net.WebUtility.HtmlEncode
                        if (member.Name.Identifier.Text == "HtmlEncode")
                        {
                            hasEncoder = true;
                            break;
                        }
                    }
                }

                if (!hasEncoder)
                    UnsafeHtmlHelpers.Add(node);
            }
        }

        base.VisitMethodDeclaration(node);
    }
}
