using CodeSheriff.SAST.Engine;
using CodeSheriff.SAST.Engine.Analyzers;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.IntegrationTests.Processors;

internal static class RedirectProcessor
{
    internal static List<BaseFinding> GetAllExternalRedirects()
    { 
        var retVal = new List<BaseFinding>();

        foreach (var project in Globals.Solution.Projects)
        {
            Globals.Compilation = project.GetCompilationAsync().Result;

            foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();
                retVal.AddRange(ExternalRedirectAnalyzer.FindProblematicExternalRedirects(root));
            }
        }

        return retVal;
    }
}
