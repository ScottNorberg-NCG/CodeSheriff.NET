using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal class FileManipulationSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    internal List<InvocationExpressionSyntax> FileManipulations = new List<InvocationExpressionSyntax>();

    public bool HasRun => FileManipulations.Any();

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var asSymbol = node.ToSymbol();

        if (asSymbol != null && asSymbol.ContainingType != null) 
        {
            if (asSymbol.ContainingType.ToString().Replace("?", "") == "System.IO.File")
            {
                if (node.Expression is MemberAccessExpressionSyntax member)
                {
                    var methodName = member.Name.Identifier.Text;

                    if (methodName.StartsWith("Append") || methodName.StartsWith("Create") || methodName.StartsWith("Open") || 
                        methodName.StartsWith("Read") || methodName.StartsWith("Write") || methodName.StartsWith("Delete") || methodName.StartsWith("Move"))
                    {
                        FileManipulations.Add(node);
                    }
                }
            }
        }

        //base.VisitInvocationExpression(node);
    }
}
