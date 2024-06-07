using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.XSS;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CodeSheriff.SAST.Engine.Analyzers;

public class HtmlHelperAnalyzer : BaseCshtmlToCodeAnalyzer
{
    public List<BaseFinding> FindXssIssues(SyntaxNode root)
    {
        var walker = new HtmlHelperSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var method in walker.UnsafeHtmlHelpers)
        {
            try
            {
                var asSymbol = method.ToSymbol() as IMethodSymbol;

                var references = asSymbol.GetMethodsReferencedIn();

                foreach (var reference in references)
                {
                    var cshtmlRoot = reference.Locations.First().SourceTree.GetRoot();
                    var referenceAsNode = cshtmlRoot.FindNode(reference.Locations.First().SourceSpan);

                    foreach (var invocationAsMethod in referenceAsNode.DescendantNodes().Where(r => r is InvocationExpressionSyntax))
                    {
                        var invocation = invocationAsMethod as InvocationExpressionSyntax;

                        if (invocation.ArgumentList.Arguments.Count == 1 && invocation.Expression is IdentifierNameSyntax id)
                        {
                            //The compiler calls Write() to write the method rather than calling our HtmlHelper method directly
                            if (id.Identifier.Text == "Write")
                            {
                                if (invocation.ArgumentList.Arguments.First().Expression is InvocationExpressionSyntax targetInvocation)
                                {
                                    if (targetInvocation.IsInvocationOfMethod(method))
                                    {
                                        GetFindingsForCshtmlInvocation(findings, targetInvocation);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex) 
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(method, ex));
            }
        }

        return findings;
    }

    protected override BaseFinding GetNewFindingForControllerAndGet()
    {
        return new HtmlHelperPropertyFromGetControllerParameter();
    }

    protected override BaseFinding GetNewFindingForControllerAndPost()
    {
        return new HtmlHelperPropertyFromOtherControllerParameter();
    }
}
