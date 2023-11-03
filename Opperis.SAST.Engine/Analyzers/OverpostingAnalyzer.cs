using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.ProgramFlow;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

        internal static List<BaseFinding> FindEFObjectsAsBindObjects(RazorPageBindObjectSyntaxWalker walker, SyntaxNode root)
        {
            if (walker.RazorPageBindObjects.Count == 0)
                walker.Visit(root);

            var findings = new List<BaseFinding>();

            foreach (var bindObject in walker.RazorPageBindObjects)
            {
                foreach (var type in Globals.EntityFrameworkObjects)
                {
                    if (bindObject.ObjectType.Equals(type))
                    {
                        var finding = new OverpostingViaBindObject();

                        finding.RootLocation = new SourceLocation(bindObject.ObjectType);

                        var callStack = new CallStack();
                        callStack.AddLocation(bindObject.ObjectType);
                        callStack.AddLocation(bindObject.ClassDeclaration);
                        finding.CallStacks.Add(callStack);

                        findings.Add(finding);
                    }
                }
            }

            return findings;
        }
    }
}
