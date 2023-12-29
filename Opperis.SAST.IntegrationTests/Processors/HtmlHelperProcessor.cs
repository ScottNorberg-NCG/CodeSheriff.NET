using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SAST.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opperis.SAST.Engine.ErrorHandling;

namespace Opperis.SAST.IntegrationTests.Processors;

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
