using CodeSheriff.SAST.Engine;
using CodeSheriff.SAST.Engine.Analyzers;
using CodeSheriff.SAST.Engine.Findings;

namespace CodeSheriff.IntegrationTests.Processors;

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
