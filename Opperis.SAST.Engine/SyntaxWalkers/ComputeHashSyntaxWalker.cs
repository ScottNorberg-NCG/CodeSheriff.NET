using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers.BaseSyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SyntaxWalkers;

internal class ComputeHashSyntaxWalker : MethodInvocationSyntaxWalker
{
    protected override List<string> MethodNames { get; } = new List<string>() { "ComputeHash" };

    protected override List<string> ObjectContainerNames => new List<string>() { "System.Security.Cryptography.HashAlgorithm" };
}
