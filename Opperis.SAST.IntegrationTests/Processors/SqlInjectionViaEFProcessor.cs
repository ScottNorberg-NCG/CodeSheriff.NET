using Opperis.SAST.Engine;
using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;

namespace Opperis.SAST.IntegrationTests.Processors;

internal static class SqlInjectionViaEFProcessor
{
    internal static List<BaseFinding> GetAllSqlInjections()
    { 
        var retVal = new List<BaseFinding>();

        foreach (var project in Globals.Solution.Projects)
        {
            Globals.Compilation = project.GetCompilationAsync().Result;

            foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();
                retVal.AddRange(SQLInjectionViaEntityFrameworkAnalyzer.GetSQLInjections(root));
            }
        }

        return retVal;
    }
}
