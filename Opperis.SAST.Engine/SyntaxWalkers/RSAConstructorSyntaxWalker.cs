using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class RSAConstructorSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<ObjectCreationExpressionSyntax> RSAConstructors { get; private set; } = new List<ObjectCreationExpressionSyntax>();

    public bool HasRun => RSAConstructors.Any();

    public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        var type = node.Type.GetUnderlyingType();

        if (type != null && type.ToString().Replace("?", "") == "System.Security.Cryptography.RSACryptoServiceProvider")
            RSAConstructors.Add(node);
        else
            base.VisitObjectCreationExpression(node);
    }
}
