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

internal class HtmlRawSyntaxWalker : MethodInvocationSyntaxWalker
{
    protected override List<string> MethodNames => new List<string>() { "Raw" };

    protected override List<string> ObjectContainerNames => new List<string>() { "IHtmlHelper", "Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper" };
}
