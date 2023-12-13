using Opperis.SAST.Engine;
using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.IntegrationTests.Processors;

internal static class SymmetricAlgorithmPropertyProcessor
{
    internal static List<BaseFinding> GetAllHardCodedKeys()
    { 
        var retVal = new List<BaseFinding>();

        foreach (var project in Globals.Solution.Projects)
        {
            Globals.Compilation = project.GetCompilationAsync().Result;

            foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();

                var walker = new SymmetricCryptographyPropertySyntaxWalker();
                retVal.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindHardCodedKeys(walker, root));
            }
        }

        return retVal;
    }

    internal static List<BaseFinding> GetAllHardCodedIVs()
    {
        var retVal = new List<BaseFinding>();

        foreach (var project in Globals.Solution.Projects)
        {
            Globals.Compilation = project.GetCompilationAsync().Result;

            foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();

                var walker = new SymmetricCryptographyPropertySyntaxWalker();

                foreach (var finding in SymmetricAlgorithmPropertyAnalyzer.FindHardCodedIVs(walker, root))
                {
                    retVal.Add(finding);
                }
            }
        }

        return retVal;
    }

    internal static List<BaseFinding> GetAllECBUses()
    {
        var retVal = new List<BaseFinding>();

        foreach (var project in Globals.Solution.Projects)
        {
            Globals.Compilation = project.GetCompilationAsync().Result;

            foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();

                var walker = new SymmetricCryptographyPropertySyntaxWalker();

                foreach (var finding in SymmetricAlgorithmPropertyAnalyzer.FindECBUses(walker, root))
                {
                    retVal.Add(finding);
                }
            }
        }

        return retVal;
    }
}
