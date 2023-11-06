using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Opperis.SAST.Engine.CompiledCSHtmlParsing;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.CSRF;
using Opperis.SAST.Engine.Findings.XSS;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers
{
    internal class HtmlRawAnalyzer : BaseCshtmlToCodeAnalyzer
    {
        internal List<BaseFinding> FindXssIssues(HtmlRawSyntaxWalker walker, SyntaxNode root)
        {
            if (!walker.HtmlRawCalls.Any())
                walker.Visit(root);

            var findings = new List<BaseFinding>();

            foreach (var call in walker.HtmlRawCalls)
            {
                GetFindingsForCshtmlInvocation(findings, call);
            }

            return findings;
        }

        protected override BaseFinding GetNewFindingForControllerAndGet()
        {
            return new HtmlRawPropertyFromGetControllerParameter();
        }

        protected override BaseFinding GetNewFindingForControllerAndPost()
        {
            return new HtmlRawPropertyFromOtherControllerParameter();
        }
    }
}
