using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.SyntaxWalkers.BaseSyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal class DatabaseConnectionCloseSyntaxWalker : MethodInvocationSyntaxWalker
{
    protected override List<string> MethodNames => new List<string>() { "Close" };

    //TODO: figure out why both versions (with namespace and without) are needed here
    protected override List<string> ObjectContainerNames => new List<string>() { "SqlConnection", "Microsoft.Data.SqlClient.SqlConnection" };
}
