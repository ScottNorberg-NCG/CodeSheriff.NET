using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.CSRF;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers;

internal static class CsrfAnalyzer
{
    //TODO: Refactor so this uses the global list
    internal static List<BaseFinding> FindCsrfIssues(SyntaxNode root)
    {
        var walker = new UIProcessorMethodSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var method in walker.ControllerMethods)
        {
            try
            {
                var methodAttributes = method.GetMethodVerbs();

                if (methodAttributes.Count() == 0)
                {
                    var finding = new ControllerActionMissingVerbAttribute();
                    SetFinding(finding, method);

                    findings.Add(finding);
                }

                //Probably a rare situation, but check if they have both a Get and a Post
                if (methodAttributes.Any(ma => ma.SkipsCsrfChecks) && methodAttributes.Any(ma => !ma.SkipsCsrfChecks))
                {
                    var finding = new ControllerActionMixingBodyAndNonBodyMethods();
                    SetFinding(finding, method);

                    findings.Add(finding);
                }

                if (methodAttributes.Any(ma => !ma.SkipsCsrfChecks))
                {
                    if (!method.HasCsrfProtection())
                    {
                        var finding = new RequestWithBodyMissingCsrfProtection();
                        SetFinding(finding, method);

                        findings.Add(finding);
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

    private static void SetFinding(BaseFinding finding, MethodDeclarationSyntax method)
    {
        var callStack = new CallStack();
        callStack.AddLocation(method);
        callStack.AddLocation(method.Parent);
        finding.CallStacks.Add(callStack);

        finding.RootLocation = new SourceLocation(method);
    }
}
