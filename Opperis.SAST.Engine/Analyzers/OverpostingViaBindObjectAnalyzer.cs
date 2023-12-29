using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.ErrorHandling;
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

namespace Opperis.SAST.Engine.Analyzers;

internal static class OverpostingViaBindObjectAnalyzer
{
    internal static List<BaseFinding> FindEFObjectsAsBindObjects(SyntaxNode root)
    {
        var walker = new RazorPageBindObjectSyntaxWalker();

        if (walker.RazorPageBindObjects.Count == 0)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var bindObject in walker.RazorPageBindObjects)
        {
            try
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
            catch (Exception ex) 
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(bindObject.ClassDeclaration, ex));
            }
        }

        return findings;
    }
}
