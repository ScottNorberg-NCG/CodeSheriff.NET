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

internal class JwtTokenParameterSetSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<AssignmentExpressionSyntax> TokenValidationParameters { get; private set; } = new List<AssignmentExpressionSyntax>();

    public bool HasRun => TokenValidationParameters.Any();

    public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        if (node.Left.ToString().In("RequireExpirationTime", "ValidateLifetime", "RequireSignedTokens"))
        {
            var parent = node.Parent;

            //Code assumes that you're using the most common method for setting the TokenValidationParameters
            //TODO: See if we need to change this,
            //  either to make it narrower (in case people are using TokenValidationParameters for other uses)
            //  or broader (people are creating the object elsewhere and adding the previously-created object)
            while (parent != null)
            {
                if (parent is ObjectCreationExpressionSyntax creation)
                {
                    var type = creation.GetUnderlyingType();
                    if (type != null && type.ToString() == "Microsoft.IdentityModel.Tokens.TokenValidationParameters")
                    {
                        TokenValidationParameters.Add(node);
                        //return;
                    }
                }

                parent = parent.Parent;
            }
        }

        base.VisitAssignmentExpression(node);
        //node.Right.Kind().ToString() == "FalseLiteralExpression"
        //node.Left.ToString() == "RequireExpirationTime"
        //(node.Parent.Parent as ObjectCreationExpressionSyntax).GetUnderlyingType().ToString() == 
    }
}
