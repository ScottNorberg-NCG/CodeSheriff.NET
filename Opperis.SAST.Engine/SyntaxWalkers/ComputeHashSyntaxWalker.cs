using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers.BaseSyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal class ComputeHashSyntaxWalker : MethodInvocationSyntaxWalker
{
    protected override List<string> MethodNames { get; } = new List<string>() { "ComputeHash" };

    protected override List<string> ObjectContainerNames => new List<string>() { "System.Security.Cryptography.HashAlgorithm" };
}
