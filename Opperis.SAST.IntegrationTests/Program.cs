using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Opperis.SAST.Engine;
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



                TestUnprotectedRedirects();
                TestSymmetricAlgorithms();
            }
        }

        private static void TestUnprotectedRedirects()
        {
            var redirects = RedirectProcessor.GetAllExternalRedirects();
            Assert.AreEqual(2, redirects.Count, "Expected number of unprotected redirects");
        }

        private static void TestSymmetricAlgorithms()
        {
            var deprecatedAlgorithms = SymmetricAlgorithmProcessor.GetAllDeprecatedAlgorithms();

            Assert.AreEqual(4, deprecatedAlgorithms.Count, "Number of deprecated algorithms");
            Assert.AreEqual(2, deprecatedAlgorithms.Count(d => d.AdditionalInformation == "Algorithm found: DESCryptoServiceProvider"), "Number of DES algorithms");
            Assert.AreEqual(2, deprecatedAlgorithms.Count(d => d.AdditionalInformation == "Algorithm found: RC2CryptoServiceProvider"), "Number of RC2 algorithms");

            foreach (var finding in deprecatedAlgorithms)
            {
                Assert.AreEqual(0, finding.CallStacks.Count, "Confirm CallStack count for SymmetricAlgorithm");
            }
        }
    }
}