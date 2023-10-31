using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Opperis.SAST.Engine;
using Opperis.SAST.Engine.Analyzers;
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

                var opens = DatabaseConnectionOpenProcessor.GetDanglingConnectionOpens();
                Assert.AreEqual(1, opens.Count, "Expected number of Unsafe Database Connection Opens");
                Assert.AreEqual(1, opens.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Database.SqlConnectionNotClosedInTryFinally"), "Number of connections opened without a using or finally block");
                Assert.AreEqual(0, opens.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Database.SqlConnectionNotClosed"), "Number of connections opened without being closed");

                TestGetUseOfECBMode();
                TestGetHardCodedIVs();
                TestGetHardCodedKeys();
                TestUnprotectedRedirects();
                TestSymmetricAlgorithms();
            }
        }

        private static void TestGetUseOfECBMode()
        {
            var ecbModes = SymmetricAlgorithmPropertyProcessor.GetAllECBUses();
            Assert.AreEqual(4, ecbModes.Count, "Expected number of ECB Modes");
            Assert.AreEqual(4, ecbModes.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Cryptography.UseOfECBMode"), "All cryptography ECB mode findings are the correct type");
        }

        private static void TestGetHardCodedIVs()
        {
            var hardCodedIVs = SymmetricAlgorithmPropertyProcessor.GetAllHardCodedIVs();
            Assert.AreEqual(4, hardCodedIVs.Count, "Expected number of hard-coded IVs");
            Assert.AreEqual(4, hardCodedIVs.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Cryptography.HardCodedCryptographyIV"), "All cryptography IV findings are the correct type");
            Assert.AreEqual(0, hardCodedIVs.Count(k => k.RootLocation.Text.Contains(",")), "No texts should include a comma (proxy for checking whether sensitive keys were removed)");
            Assert.AreEqual(0, hardCodedIVs.SelectMany(k => k.CallStacks).SelectMany(c => c.Locations).Count(l => l.Text.Contains(",")), "No call stack texts should include a comma (proxy for checking whether sensitive keys were removed)");
        }

        private static void TestGetHardCodedKeys()
        {
            var hardCodedKeys = SymmetricAlgorithmPropertyProcessor.GetAllHardCodedKeys();
            Assert.AreEqual(4, hardCodedKeys.Count, "Expected number of hard-coded keys");
            Assert.AreEqual(4, hardCodedKeys.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.Cryptography.HardCodedCryptographyKey"), "All cryptography key findings are the correct type");
            Assert.AreEqual(0, hardCodedKeys.Count(k => k.RootLocation.Text.Contains(",")), "No texts should include a comma (proxy for checking whether sensitive keys were removed)");
            Assert.AreEqual(0, hardCodedKeys.SelectMany(k => k.CallStacks).SelectMany(c => c.Locations).Count(l => l.Text.Contains(",")), "No call stack texts should include a comma (proxy for checking whether sensitive keys were removed)");
        }

        private static void TestUnprotectedRedirects()
        {
            var redirects = RedirectProcessor.GetAllExternalRedirects();
            Assert.AreEqual(2, redirects.Count, "Expected number of unprotected redirects");
            Assert.AreEqual(2, redirects.Count(r => r.GetType().ToString() == "Opperis.SAST.Engine.Findings.ProgramFlow.UnprotectedExternalRedirect"), "All external redirects are the correct type");
        }

        private static void TestSymmetricAlgorithms()
        {
            var deprecatedAlgorithms = SymmetricAlgorithmProcessor.GetAllDeprecatedAlgorithms();

            Assert.AreEqual(4, deprecatedAlgorithms.Count, "Number of deprecated algorithms");
            Assert.AreEqual(4, deprecatedAlgorithms.Count(a => a.GetType().ToString() == "Opperis.SAST.Engine.Findings.Cryptography.UseOfDeprecatedAlgorithm"), "All deprecated algorithms have the correct object type");
            Assert.AreEqual(2, deprecatedAlgorithms.Count(d => d.AdditionalInformation == "Algorithm found: DESCryptoServiceProvider"), "Number of DES algorithms");
            Assert.AreEqual(2, deprecatedAlgorithms.Count(d => d.AdditionalInformation == "Algorithm found: RC2CryptoServiceProvider"), "Number of RC2 algorithms");

            foreach (var finding in deprecatedAlgorithms)
            {
                Assert.AreEqual(0, finding.CallStacks.Count, "Confirm CallStack count for SymmetricAlgorithm");
            }
        }
    }
}