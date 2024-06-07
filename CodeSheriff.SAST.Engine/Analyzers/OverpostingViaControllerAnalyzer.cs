using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.ProgramFlow;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class OverpostingViaControllerAnalyzer
{
    public static List<BaseFinding> FindEFObjectsAsParameters()
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
