using Microsoft.CodeAnalysis;
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

internal class ValueShadowingAnalyzer
{
    internal static List<BaseFinding> FindValueShadowingPossibilities(UIProcessorMethodSyntaxWalker walker, SyntaxNode root)
    {
        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var method in walker.ControllerMethods)
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
