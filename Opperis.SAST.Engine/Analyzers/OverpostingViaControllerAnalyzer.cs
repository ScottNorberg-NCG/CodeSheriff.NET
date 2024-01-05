using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.ProgramFlow;
using Opperis.SAST.Engine.RoslynObjectExtensions;

namespace Opperis.SAST.Engine.Analyzers;

internal static class OverpostingViaControllerAnalyzer
{
    internal static List<BaseFinding> FindEFObjectsAsParameters()
    {
        var findings = new List<BaseFinding>();

        foreach (var method in Globals.SolutionControllerMethods)
        {
            try
            {
                foreach (var parameter in method.ParameterList.Parameters)
                {
                    foreach (var type in Globals.EntityFrameworkObjects)
                    {
                        if (type.Equals(parameter.Type.GetUnderlyingType()))
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
            catch (Exception ex) 
            { 
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(method, ex));
            }
        }

        return findings;
    }
}
