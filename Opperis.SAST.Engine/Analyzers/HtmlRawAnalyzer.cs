using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.XSS;
using Opperis.SAST.Engine.SyntaxWalkers;

namespace Opperis.SAST.Engine.Analyzers;

internal class HtmlRawAnalyzer : BaseCshtmlToCodeAnalyzer
{
    internal List<BaseFinding> FindXssIssues(HtmlRawSyntaxWalker walker, SyntaxNode root)
    {
        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var call in walker.MethodCalls)
        {
            try
            { 
                GetFindingsForCshtmlInvocation(findings, call);
            }
            catch (Exception ex) 
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(call, ex));
            }
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
