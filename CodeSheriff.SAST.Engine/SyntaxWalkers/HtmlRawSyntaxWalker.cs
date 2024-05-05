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

internal class HtmlRawSyntaxWalker : MethodInvocationSyntaxWalker
{
    protected override List<string> MethodNames => new List<string>() { "Raw" };

    protected override List<string> ObjectContainerNames => new List<string>() { "IHtmlHelper", "Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper" };
}
