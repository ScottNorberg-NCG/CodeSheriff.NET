using Microsoft.CodeAnalysis;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.ProgramFlow;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class OverpostingViaBindObjectAnalyzer
{
    public static List<BaseFinding> FindEFObjectsAsBindObjects(SyntaxNode root)
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
