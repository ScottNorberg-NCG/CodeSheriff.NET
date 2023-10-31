using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.CSRF;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers
{
    internal class ValueShadowingAnalyzer
    {
        internal static List<BaseFinding> FindValueShadowingPossibilities(ControllerMethodSyntaxWalker walker, SyntaxNode root)
        {
            if (!walker.Methods.Any())
                walker.Visit(root);

            var findings = new List<BaseFinding>();

            foreach (var method in walker.Methods)
            {
                foreach (var parameter in method.ParameterList.Parameters)
                {
                    if (!parameter.HasBindingSourceInfo())
                    {
                        var finding = new ControllerParameterMissingBindingInfo();

                        finding.RootLocation = new SourceLocation(parameter);

                        var callStack = new CallStack();
                        callStack.Locations.Add(new SourceLocation(parameter));
                        callStack.Locations.Add(new SourceLocation(method));
                        callStack.Locations.Add(new SourceLocation(method.Parent));

                        finding.CallStacks.Add(callStack);

                        findings.Add(finding);
                    }
                }
            }

            return findings;
        }
    }
}
