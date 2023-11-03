using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.ProgramFlow;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers
{
    internal static class OverpostingAnalyzer
    {
        internal static List<BaseFinding> FindEFObjectsAsParameters()
        {
            var findings = new List<BaseFinding>();

            foreach (var method in Globals.SolutionControllerMethods)
            {
                foreach (var parameter in method.ParameterList.Parameters)
                {
                    foreach (var type in Globals.EntityFrameworkObjects)
                    {
                        if (parameter.Type.GetUnderlyingType().Equals(type))
                        {
                            var finding = new OverpostingViaControllerMethod();

                            finding.RootLocation = new SourceLocation(parameter);

                            var callStack = new CallStack();
                            callStack.AddLocation(parameter);
                            callStack.AddLocation(method);
                            finding.CallStacks.Add(callStack);

                            findings.Add(finding);
                        }
                    }
                }
            }

            return findings;
        }
    }
}
