using CodeSheriff.SAST.Engine.Analyzers;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using CodeSheriff.SAST.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSheriff.SAST.Engine.ErrorHandling;

namespace CodeSheriff.IntegrationTests.Processors;

internal static class HtmlHelperProcessor
{
    internal static List<BaseFinding> GetXssIssues()
    {
        var retVal = new List<BaseFinding>();

        foreach (var project in Globals.Solution.Projects)
        {
            Globals.Compilation = project.GetCompilationAsync().Result;

            foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();

                var analyzer = new HtmlHelperAnalyzer();
                retVal.AddRange(analyzer.FindXssIssues(root));
            }
        }

        return retVal;
    }
}
