using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.CSRF;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class CsrfAnalyzer
{
    //TODO: Refactor so this uses the global list
    public static List<BaseFinding> FindCsrfIssues()
    {
        var findings = new List<BaseFinding>();

        foreach (var method in Globals.SolutionControllerMethods)
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
