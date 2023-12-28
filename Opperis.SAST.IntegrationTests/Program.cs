using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Opperis.SAST.Engine;
using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SCA;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SAST.IntegrationTests.Processors;
using Opperis.SCA.Engine;
using System.Diagnostics;
using System.Xml.Linq;

namespace Opperis.SAST.IntegrationTests;

internal class Program
{
    static void Main(string[] args)
    {
        //var solutionFilePath = "C:\\Users\\scott\\Source\\repos\\VulnerabilityBuffet2\\AspNetCore\\NCG.SecurityDetection.VulnerabilityBuffet.sln";
        //var solutionFilePath = "C:\\Users\\scott\\Downloads\\WebGoat.NETCore-master\\WebGoat.NET-master\\WebGoat.NET.sln";
        //var solutionFilePath = "C:\\Users\\scott\\Downloads\\sentry-dotnet-main\\sentry-dotnet-main\\Sentry.NoMobile.sln";
        //var solutionFilePath = "C:\\Users\\scott\\Source\\repos\\Opperis.IAST\\Opperis.IAST.sln";
        var solutionFilePath = "C:\\Users\\scott\\Source\\repos\\SASTTest\\SASTTest.sln";

        if (!MSBuildLocator.IsRegistered)
            MSBuildLocator.RegisterDefaults();

        using (var workspace = MSBuildWorkspace.Create())
        {
            Globals.Solution = workspace.OpenSolutionAsync(solutionFilePath).Result;

            var findings = new List<BaseFinding>();
            //foreach (var project in Globals.Solution.Projects)
            //{
            //    Globals.Compilation = project.GetCompilationAsync().Result;

            //    foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            //    {
            //        var root = syntaxTree.GetRoot();

            //        var walker = new IUserStoreSyntaxWalker();
            //        walker.Visit(root);

            //        //findings.AddRange(SQLInjectionViaEntityFrameworkAnalyzer.GetSQLInjections(walker, syntaxTree.GetRoot()));
            //        int i = 1;
            //    }
            //}

            TestFileResultIssues();
            TestIUserStoreMisconfigurations();
            TestPasswordLockouts();
            TestSqlInjectionsViaEF();
            TestJwtConfigurationIssues();
            TestHashAlgorithmIssues();
            TestRSAKeyLengthIssues();
            TestModelValidationIssues();
            //TestTrufflehogFindings();
            TestGetStoredSecrets();
            TestGetScaIssues();
            TestGetStoredSecrets();
            TestFileManipulationIssues();
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

            Console.WriteLine($"Scan completed with {Globals.RuntimeErrors.Count} errors");
        }
    }

    private static void TestFileResultIssues()
    {
        var findings = FileResultProcessor.GetFileManipulations();
        //WriteFindingsToConsole(xssIssues);
        Assert.AreEqual(2, findings.Count, "Expected number of file result issues");
        Assert.AreEqual(2, findings.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of file result issues");
        Assert.AllRootLocationsSet(findings, "TestFileResultIssues");
    }

    private static void TestIUserStoreMisconfigurations()
    {
        var findings = IUserStoreProcessor.GetIUserStoreMisconfigurations();
        Assert.AreEqual(1, findings.Count, "Number of IUserStore issues");
        Assert.AllRootLocationsSet(findings, "TestIUserStoreMisconfigurations");
    }

    private static void TestPasswordLockouts()
    {
        var findings = PasswordSignInProcessor.GetIssues();
        Assert.AreEqual(1, findings.Count, "Number of password lockout issues");
        Assert.AllRootLocationsSet(findings, "TestPasswordLockouts");
    }

    private static void TestSqlInjectionsViaEF()
    {
        var findings = SqlInjectionViaEFProcessor.GetAllSqlInjections();
        Assert.AreEqual(3, findings.Count, "Expected number of Sql Injections via EF methods");
        Assert.AllRootLocationsSet(findings, "TestSqlInjectionsViaEF");
    }

    private static void TestJwtConfigurationIssues()
    {
        var findings = JwtConfigurationProcessor.GetAllMisconfiguredProperties();
        //WriteFindingsToConsole(xssIssues);
        Assert.AreEqual(3, findings.Count, "Expected number of JWT configuration issues");
        Assert.AreEqual(3, findings.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of JWT configuration issues");
        Assert.AllRootLocationsSet(findings, "TestJwtConfigurationIssues");
    }

    private static void TestHashAlgorithmIssues()
    {
        var findings = HashAlgorithmProcessor.GetDeprecatedAlgorithms();
        Assert.AreEqual(4, findings.Count, "Expected number of deprecated hash uses");
        Assert.AreEqual(4, findings.Select(c => c.AdditionalInformation).Distinct().Count(), "Number of distinct deprecated hashes");
        Assert.AllRootLocationsSet(findings, "TestHashAlgorithmIssues");
    }

    private static void TestRSAKeyLengthIssues()
    {
        var constructorFindings = RSAKeySizeInConstructorProcessor.GetAllMisconfiguredConstructors();
        Assert.AreEqual(3, constructorFindings.Count, "Expected number of RSA key size in constructor issues");
        Assert.AllRootLocationsSet(constructorFindings, "TestRSAKeyLengthIssues (Constructor)");

        var propertyFindings = RSAKeySizeInPropertyProcessor.GetAllMisconfiguredProperties();
        Assert.AreEqual(2, propertyFindings.Count, "Expected number of RSA key size in property issues");
        Assert.AllRootLocationsSet(propertyFindings, "TestRSAKeyLengthIssues (Property)");
    }

    private static void TestModelValidationIssues()
    {
        var modelErrors = ModelValidationProcessor.GetAllModelsMissingValidation();
        Assert.AreEqual(13, modelErrors.Count, "Expected number of model validation issues");
        Assert.AreEqual(4, modelErrors.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of model validation issues");
        Assert.AllRootLocationsSet(modelErrors, "TestModelValidationIssues");
    }

    private static void TestTrufflehogFindings()
    {
        var truffleHogFindings = TrufflehogProcessor.ProcessSolution();
        Assert.AreEqual(5, truffleHogFindings.Count, "Expected number of Trufflehog findings");
        Assert.AllRootLocationsSet(truffleHogFindings, "TestTrufflehogFindings");
    }

    private static void TestGetScaIssues()
    {
        var issues = ScaProcessor.GetScaIssues();
        Assert.IsTrue(issues.Count > 0, "There should be more than one SCA issue found");
    }

    private static void TestGetStoredSecrets()
    {
        //var storedSecrets = SecretStorageProcessor.GetStoredSecrets();
        //Assert.AreEqual(5, storedSecrets.Count, "Expected number of stored secrets");
        //Assert.AreEqual(5, storedSecrets.Select(c => c.Description).Distinct().Count(), "Number of distinct types of stored secrets");
        //Assert.AllRootLocationsSet(storedSecrets, "TestGetStoredSecrets");
    }

    private static void TestFileManipulationIssues()
    {
        var fileManipulations = FileManipulationProcessor.GetFileManipulations();
        //WriteFindingsToConsole(fileManipulations);
        Assert.AreEqual(2, fileManipulations.Count, "Expected number of file manipulation issues");
        Assert.AreEqual(2, fileManipulations.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of file manipulation issues");
        Assert.AllRootLocationsSet(fileManipulations, "TestFileManipulationIssues");
    }

    private static void TestCookieConfigurationIssues()
    {
        var cookieIssues = CookieConfigurationProcessor.GetCookieConfigurationIssues();
        //WriteFindingsToConsole(cookieIssues);
        Assert.AreEqual(6, cookieIssues.Count, "Expected number of cookie configuration issues");
        Assert.AreEqual(5, cookieIssues.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of cookie configuration issues");
        Assert.AllRootLocationsSet(cookieIssues, "TestCookieConfigurationIssues");
    }

    private static void TestHtmlHelperXssIssues()
    {
        var xssIssues = HtmlHelperProcessor.GetXssIssues();
        //WriteFindingsToConsole(xssIssues);
        Assert.AreEqual(2, xssIssues.Count, "Expected number of XSS Issues from HtmlHelper calls");
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
        Assert.AreEqual(1, xssIssues.Count, "Expected number of XSS Issues from Html.Raw calls");
        Assert.AreEqual(1, xssIssues.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of XSS issues from Html.Raw calls");
        Assert.AllRootLocationsSet(xssIssues, "TestHtmlRawXssIssues");
    }

    private static void TestHardCodedConnectionStrings()
    {
        var connectionStrings = HardCodedConnectionStringProcessor.GetConnectionStrings();
        Assert.AreEqual(4, connectionStrings.Count, "Number of hard-coded connection strings");
        Assert.AllRootLocationsSet(connectionStrings, "TestHardCodedConnectionStrings");
    }

    private static void TestSqlInjections()
    {
        var sqlInjections = SqlInjectionProcessor.GetSqlInjections();
        Assert.AreEqual(10, sqlInjections.Count, "Expected number of SQL Injection issues");
        Assert.AreEqual(2, sqlInjections.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of SQL Injection issues");
        Assert.AreEqual(2, sqlInjections.Max(s => s.CallStacks.Count), "ExecSql has two different callstacks");
        Assert.AreEqual(6, sqlInjections.SelectMany(s => s.CallStacks).Max(cs => cs.Locations.Count), "SQL injection - number of locations in the largest callstack");
        Assert.AllRootLocationsSet(sqlInjections, "TestSqlInjections");
    }

    private static void TestValueShadowingIssues()
    {
        //Number of Value Shadowing issues will change frequently
        //If an issue is found here, check to see whether the project changed first before debugging tests
        var valueShadowing = ValueShadowingProcessor.GetValueShadowingIssues();
        Assert.AreEqual(32, valueShadowing.Count, "Expected number of Value Shadowing Issues");
        Assert.AllRootLocationsSet(valueShadowing, "TestValueShadowingIssues");
    }

    private static void TestCsrfIssues()
    {
        //Number of CSRF issues will change frequently
        //If an issue is found here, check to see whether the project changed first before debugging tests
        var csrfIssues = CsrfProcessor.GetCsrfIssues();
        Assert.AreEqual(32, csrfIssues.Count, "Expected number of Csrf Issues");
        Assert.AreEqual(3, csrfIssues.Select(c => c.GetType().ToString()).Distinct().Count(), "Number of distinct types of CSRF issues");
        Assert.AllRootLocationsSet(csrfIssues, "TestCsrfIssues");
    }

    private static void TestDanglingConnectionOpens()
    {
        var opens = DatabaseConnectionOpenProcessor.GetDanglingConnectionOpens();
        Assert.AreEqual(2, opens.Count, "Expected number of Unsafe Database Connection Opens");
        Assert.AreEqual(1, opens.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Database.SqlConnectionNotClosedInTryFinally"), "Number of connections opened without a using or finally block");
        Assert.AreEqual(1, opens.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Database.SqlConnectionNotClosed"), "Number of connections opened without being closed");
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
        Assert.AreEqual(4, redirects.Count, "Expected number of unprotected redirects");
        Assert.AllRootLocationsSet(redirects, "TestUnprotectedRedirects");
    }

    private static void TestSymmetricAlgorithms()
    {
        var deprecatedAlgorithms = SymmetricAlgorithmProcessor.GetAllDeprecatedAlgorithms();

        Assert.AreEqual(4, deprecatedAlgorithms.Count, "Number of deprecated algorithms");
        Assert.AreEqual(4, deprecatedAlgorithms.Count(a => a.GetType().ToString() == "Opperis.SAST.Engine.Findings.Cryptography.UseOfDeprecatedSymmetricAlgorithm"), "All deprecated algorithms have the correct object type");
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