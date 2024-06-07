using Microsoft.CodeAnalysis;
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

public class ValueShadowingAnalyzer
{
    public static List<BaseFinding> FindValueShadowingPossibilities()
    {
        var findings = new List<BaseFinding>();

        foreach (var method in Globals.SolutionControllerMethods)
        {
            try
            {
                foreach (var parameter in method.ParameterList.Parameters)
                {
                    if (!parameter.HasBindingSourceInfo())
                    {
                        var finding = new ControllerParameterMissingBindingInfo();

                        finding.RootLocation = new SourceLocation(parameter);

                        var callStack = new CallStack();
                        callStack.AddLocation(parameter);
                        callStack.AddLocation(method);
                        callStack.AddLocation(method.Parent);

                        finding.CallStacks.Add(callStack);

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
}
