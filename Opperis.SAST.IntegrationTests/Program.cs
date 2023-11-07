using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Opperis.SAST.Engine;
using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SAST.IntegrationTests.Processors;

namespace Opperis.SAST.IntegrationTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var solutionFilePath = "C:\\Users\\scott\\Source\\repos\\VulnerabilityBuffet2\\AspNetCore\\NCG.SecurityDetection.VulnerabilityBuffet.sln";

            if (!MSBuildLocator.IsRegistered)
                MSBuildLocator.RegisterDefaults();

            using (var workspace = MSBuildWorkspace.Create())
            {
                Globals.Solution = workspace.OpenSolutionAsync(solutionFilePath).Result;

                //foreach (var project in Globals.Solution.Projects)
                //{
                //    Globals.Compilation = project.GetCompilationAsync().Result;

                //    foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
                //    {
                //        var root = syntaxTree.GetRoot();

                //        var walker = new CookieAppendSyntaxWalker();
                //        walker.Visit(root);
                //    }
                //}

                TestCookieConfigurationIssues();
                TestHtmlHelperXssIssues();
                TestOverpostingInRazorPages();
                TestOverpostingInControllers();
                TestHtmlRawXssIssues();
                TestHardCodedConnectionStrings();
                TestSqlInjections();
                TestValueShadowingIssues();
                TestCsrfIssues();
                TestDanglingConnectionOpens();
                TestGetUseOfECBMode();
                TestGetHardCodedIVs();
                TestGetHardCodedKeys();
                TestUnprotectedRedirects();
                TestSymmetricAlgorithms();
            }
        }

        private static void TestCookieConfigurationIssues()
        {
            var cookieIssues = CookieConfigurationProcessor.GetCookieConfigurationIssues();
            //WriteFindingsToConsole(cookieIssues);
            Assert.AreEqual(11, cookieIssues.Count, "Expected number of cookie configuration issues");
            Assert.AreEqual(5, cookieIssues.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of cookie configuration issues");
            Assert.AllRootLocationsSet(cookieIssues, "TestCookieConfigurationIssues");
        }

        private static void TestHtmlHelperXssIssues()
        {
            var xssIssues = HtmlHelperProcessor.GetXssIssues();
            //WriteFindingsToConsole(xssIssues);
            Assert.AreEqual(1, xssIssues.Count, "Expected number of XSS Issues from HtmlHelper calls");
            Assert.AllRootLocationsSet(xssIssues, "TestHtmlHelperXssIssues");
        }

        private static void TestOverpostingInRazorPages()
        {
            var efObjectsAsBindObjects = OverpostingProcessor.GetOverpostingIssues();
            Assert.AreEqual(1, efObjectsAsBindObjects.Count, "Expected number of Overposting in razor pages");
            Assert.AllRootLocationsSet(efObjectsAsBindObjects, "TestOverpostingInRazorPages");
        }

        private static void TestOverpostingInControllers()
        {
            var efObjectsAsParameters = OverpostingAnalyzer.FindEFObjectsAsParameters();
            Assert.AreEqual(1, efObjectsAsParameters.Count, "Expected number of Overposting in controllers");
            Assert.AllRootLocationsSet(efObjectsAsParameters, "TestOverpostingInControllers");
        }

        private static void TestHtmlRawXssIssues()
        {
            var xssIssues = HtmlRawProcessor.GetXssIssues();
            //WriteFindingsToConsole(xssIssues);
            Assert.AreEqual(2, xssIssues.Count, "Expected number of XSS Issues from Html.Raw calls");
            Assert.AreEqual(1, xssIssues.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of XSS issues from Html.Raw calls");
            Assert.AllRootLocationsSet(xssIssues, "TestHtmlRawXssIssues");
        }

        private static void TestHardCodedConnectionStrings()
        {
            var connectionStrings = HardCodedConnectionStringProcessor.GetConnectionStrings();
            Assert.AreEqual(2, connectionStrings.Count, "Number of hard-coded connection strings");
            Assert.AllRootLocationsSet(connectionStrings, "TestHardCodedConnectionStrings");
        }

        private static void TestSqlInjections()
        {
            var sqlInjections = SqlInjectionProcessor.GetSqlInjections();
            Assert.AreEqual(8, sqlInjections.Count, "Expected number of SQL Injection Issues");
            Assert.AreEqual(2, sqlInjections.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of SQL Injection issues");
            Assert.AreEqual(49, sqlInjections.Max(s => s.CallStacks.Count), "ExecSql has many different callstacks");
            Assert.AreEqual(7, sqlInjections.SelectMany(s => s.CallStacks).Max(cs => cs.Locations.Count), "SQL injection - number of locations in the largest callstack");
            Assert.AllRootLocationsSet(sqlInjections, "TestSqlInjections");
        }

        private static void TestValueShadowingIssues()
        {
            //Number of Value Shadowing issues will change frequently
            //If an issue is found here, check to see whether the project changed first before debugging tests
            var valueShadowing = ValueShadowingProcessor.GetValueShadowingIssues();
            Assert.AreEqual(64, valueShadowing.Count, "Expected number of Value Shadowing Issues");
            Assert.AllRootLocationsSet(valueShadowing, "TestValueShadowingIssues");
        }

        private static void TestCsrfIssues()
        {
            //Number of CSRF issues will change frequently
            //If an issue is found here, check to see whether the project changed first before debugging tests
            var csrfIssues = CsrfProcessor.GetCsrfIssues();
            Assert.AreEqual(58, csrfIssues.Count, "Expected number of Csrf Issues");
            Assert.AreEqual(3, csrfIssues.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of CSRF issues");
            Assert.AllRootLocationsSet(csrfIssues, "TestCsrfIssues");
        }

        private static void TestDanglingConnectionOpens()
        {
            var opens = DatabaseConnectionOpenProcessor.GetDanglingConnectionOpens();
            Assert.AreEqual(1, opens.Count, "Expected number of Unsafe Database Connection Opens");
            Assert.AreEqual(1, opens.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Database.SqlConnectionNotClosedInTryFinally"), "Number of connections opened without a using or finally block");
            Assert.AreEqual(0, opens.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Database.SqlConnectionNotClosed"), "Number of connections opened without being closed");
            Assert.AllRootLocationsSet(opens, "TestDanglingConnectionOpens");
        }

        private static void TestGetUseOfECBMode()
        {
            var ecbModes = SymmetricAlgorithmPropertyProcessor.GetAllECBUses();
            Assert.AreEqual(4, ecbModes.Count, "Expected number of ECB Modes");
            Assert.AreEqual(4, ecbModes.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Cryptography.UseOfECBMode"), "All cryptography ECB mode findings are the correct type");
            Assert.AllRootLocationsSet(ecbModes, "TestGetUseOfECBMode");
        }

        private static void TestGetHardCodedIVs()
        {
            var hardCodedIVs = SymmetricAlgorithmPropertyProcessor.GetAllHardCodedIVs();
            Assert.AreEqual(4, hardCodedIVs.Count, "Expected number of hard-coded IVs");
            Assert.AreEqual(4, hardCodedIVs.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Cryptography.HardCodedCryptographyIV"), "All cryptography IV findings are the correct type");
            Assert.AreEqual(0, hardCodedIVs.Count(k => k.RootLocation.Text.Contains(",")), "No texts should include a comma (proxy for checking whether sensitive keys were removed)");
            Assert.AreEqual(0, hardCodedIVs.SelectMany(k => k.CallStacks).SelectMany(c => c.Locations).Count(l => l.Text.Contains(",")), "No call stack texts should include a comma (proxy for checking whether sensitive keys were removed)");
            Assert.AllRootLocationsSet(hardCodedIVs, "TestGetHardCodedIVs");
        }

        private static void TestGetHardCodedKeys()
        {
            var hardCodedKeys = SymmetricAlgorithmPropertyProcessor.GetAllHardCodedKeys();
            Assert.AreEqual(4, hardCodedKeys.Count, "Expected number of hard-coded keys");
            Assert.AreEqual(4, hardCodedKeys.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Cryptography.HardCodedCryptographyKey"), "All cryptography key findings are the correct type");
            Assert.AreEqual(0, hardCodedKeys.Count(k => k.RootLocation.Text.Contains(",")), "No texts should include a comma (proxy for checking whether sensitive keys were removed)");
            Assert.AreEqual(0, hardCodedKeys.SelectMany(k => k.CallStacks).SelectMany(c => c.Locations).Count(l => l.Text.Contains(",")), "No call stack texts should include a comma (proxy for checking whether sensitive keys were removed)");
            Assert.AllRootLocationsSet(hardCodedKeys, "TestGetHardCodedKeys");
        }

        private static void TestUnprotectedRedirects()
        {
            var redirects = RedirectProcessor.GetAllExternalRedirects();
            Assert.AreEqual(2, redirects.Count, "Expected number of unprotected redirects");
            Assert.AreEqual(2, redirects.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.ProgramFlow.UnprotectedExternalRedirect"), "All external redirects are the correct type");
            Assert.AllRootLocationsSet(redirects, "TestUnprotectedRedirects");
        }

        private static void TestSymmetricAlgorithms()
        {
            var deprecatedAlgorithms = SymmetricAlgorithmProcessor.GetAllDeprecatedAlgorithms();

            Assert.AreEqual(4, deprecatedAlgorithms.Count, "Number of deprecated algorithms");
            Assert.AreEqual(4, deprecatedAlgorithms.Count(a => a.GetType().ToString() == "Opperis.SAST.Engine.Findings.Cryptography.UseOfDeprecatedAlgorithm"), "All deprecated algorithms have the correct object type");
            Assert.AreEqual(2, deprecatedAlgorithms.Count(d => d.AdditionalInformation == "Algorithm found: DESCryptoServiceProvider"), "Number of DES algorithms");
            Assert.AreEqual(2, deprecatedAlgorithms.Count(d => d.AdditionalInformation == "Algorithm found: RC2CryptoServiceProvider"), "Number of RC2 algorithms");
            Assert.AllRootLocationsSet(deprecatedAlgorithms, "TestSymmetricAlgorithms");

            foreach (var finding in deprecatedAlgorithms)
            {
                Assert.AreEqual(0, finding.CallStacks.Count, "Confirm CallStack count for SymmetricAlgorithm");
            }
        }

        private static void WriteFindingsToConsole(List<BaseFinding> findings)
        {
            foreach (var finding in findings)
            {
                Console.WriteLine($"New Finding: {finding.FindingText}");
                Console.WriteLine($"Root Location: {finding.RootLocation}");
                
                foreach (var cs in finding.CallStacks)
                { 
                    Console.WriteLine("Call Stack: ");

                    foreach (var location in cs.Locations)
                    {
                        Console.WriteLine(location.ToString());
                    }

                    Console.WriteLine("-----");
                }

                Console.WriteLine("*********************************");
            }
        }
    }
}