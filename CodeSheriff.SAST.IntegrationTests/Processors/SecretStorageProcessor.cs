using CodeSheriff.SAST.Engine.Analyzers;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using CodeSheriff.SAST.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.IntegrationTests.Processors;

internal static class SecretStorageProcessor
{
    internal static List<BaseFinding> GetStoredSecrets()
    {
        var retVal = new List<BaseFinding>();
        var rules = CodeSheriff.Secrets.RulesEngine.GetGitLeaksRules();

        foreach (var project in Globals.Solution.Projects)
        {
            Globals.Compilation = project.GetCompilationAsync().Result;

            foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();
                retVal.AddRange(SecretStorageAnalyzer.GetStoredSecrets(root, rules));
            }
        }

        return retVal;
    }
}
