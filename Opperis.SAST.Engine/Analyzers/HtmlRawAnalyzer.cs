using Microsoft.CodeAnalysis;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.XSS;
using CodeSheriff.SAST.Engine.SyntaxWalkers;

namespace CodeSheriff.SAST.Engine.Analyzers;

internal class HtmlRawAnalyzer : BaseCshtmlToCodeAnalyzer
{
    internal List<BaseFinding> FindXssIssues(SyntaxNode root)
    {
        var walker = new HtmlRawSyntaxWalker();

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
