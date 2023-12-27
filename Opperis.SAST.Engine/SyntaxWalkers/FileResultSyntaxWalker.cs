using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class FileResultSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<ObjectCreationExpressionSyntax> FileResults = new List<ObjectCreationExpressionSyntax>();

    public bool HasRun => FileResults.Any();

    public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        var type = node.GetUnderlyingType();
        if (type != null && type.ToString().Replace("?", "").In("Microsoft.AspNetCore.Mvc.PhysicalFileResult", "Microsoft.AspNetCore.Mvc.VirtualFileResult"))
        {
            FileResults.Add(node);
        }

        base.VisitObjectCreationExpression(node);
    }
}
