using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal class ReturnStatementSyntaxWalker : CSharpSyntaxWalker, ISyntaxWalker
{
    public List<ReturnStatementSyntax> ReturnStatements = new List<ReturnStatementSyntax>();

    public bool HasRun => ReturnStatements.Any();

    public override void VisitReturnStatement(ReturnStatementSyntax node)
    {
        ReturnStatements.Add(node);
        base.VisitReturnStatement(node);
    }
}
