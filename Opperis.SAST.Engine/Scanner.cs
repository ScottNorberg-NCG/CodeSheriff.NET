using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.HtmlTagParsing;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine
{
    internal static class Scanner
    {
        internal static List<BaseFinding> Scan(string solutionFilePath)
        {
            if (!MSBuildLocator.IsRegistered)
                MSBuildLocator.RegisterDefaults();

            var findings = new List<BaseFinding>();

            using (var workspace = MSBuildWorkspace.Create())
            {
                Globals.Solution = workspace.OpenSolutionAsync(solutionFilePath).Result;

                foreach (var project in Globals.Solution.Projects)
                {
                    Globals.Compilation = project.GetCompilationAsync().Result;

                    foreach (var cshtmlFile in project.AdditionalDocuments.Where(d => d.FilePath.EndsWith(".cshtml")))
                    {
                        var scripts = CSHtmlScriptTagParser.GetScriptTags(cshtmlFile.GetTextAsync().Result.ToString());
                        findings.AddRange(CSHtmlScriptTagParser.ParseJavaScriptFindings(scripts, cshtmlFile));

                        var links = CSHtmlLinkTagParser.GetLinkTags(cshtmlFile.GetTextAsync().Result.ToString());
                        findings.AddRange(CSHtmlLinkTagParser.ParseLinkTagFindings(links, cshtmlFile));

                        var styleTags = CSHtmlStyleTagParser.GetStyleTags(cshtmlFile.GetTextAsync().Result.ToString());
                        findings.AddRange(CSHtmlStyleTagParser.ParseStyleTagFindings(styleTags, cshtmlFile));
                    }

                    foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
                    {
                        var root = syntaxTree.GetRoot();

                        var databaseCalls = new DatabaseCommandTextSyntaxWalker();
                        findings.AddRange(SQLInjectionAnalyzer.GetSQLInjections(databaseCalls, root));

                        var controllerMethods = new ControllerMethodSyntaxWalker();
                        findings.AddRange(CsrfAnalyzer.FindCsrfIssues(controllerMethods, root));
                        findings.AddRange(ValueShadowingAnalyzer.FindValueShadowingPossibilities(controllerMethods, root));

                        var databaseConnections = new DatabaseConnectionOpenSyntaxWalker();
                        findings.AddRange(DatabaseConnectionOpenAnalyzer.FindUnsafeDatabaseConnectionOpens(databaseConnections, root));

                        var cryptoKeyFinder = new SymmetricCryptographyPropertySyntaxWalker();
                        findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindHardCodedKeys(cryptoKeyFinder, root));
                        findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindHardCodedIVs(cryptoKeyFinder, root));
                        findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindECBUses(cryptoKeyFinder, root));

                        var cryptoAlgorithmFinder = new SymmetricAlgorithmSyntaxWalker();
                        findings.AddRange(SymmetricAlgorithmAnalyzer.FindDeprecatedAlgorithms(cryptoAlgorithmFinder, root));

                        var externalRedirects = new ExternalRedirectSyntaxWalker();
                        findings.AddRange(ExternalRedirectAnalyzer.FindProblematicExternalRedirects(externalRedirects, root));

                        var hardCodedConnectionStrings = new DatabaseConnectionStringSyntaxWalker();
                        findings.AddRange(HardCodedConnectionStringAnalyzer.FindHardCodedConnectionStrings(hardCodedConnectionStrings, root));
                    }
                }
            }

            return findings;
        }
    }
}
