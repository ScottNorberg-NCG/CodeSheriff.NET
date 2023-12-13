using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings.ProgramFlow;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.Findings.SCA;
using Opperis.SCA.Engine;

namespace Opperis.SAST.Engine.Analyzers;

internal static class ScaAnalyzer
{
    internal static List<BaseFinding> GetVulnerableNuGetPackages()
    {
        var findings = new List<BaseFinding>();

        foreach (var reference in Globals.NuGetReferences)
        {
            try
            {
                var nuGetInfo = NuGetLoader.GetNuGetInfo(reference.AssemblyName);

                var nuGetEntry = nuGetInfo.SingleOrDefault(i => i.version == reference.VersionString);

                if (nuGetEntry == null)
                    continue;

                if (nuGetEntry.vulnerabilities.Count > 0)
                {
                    var severity = nuGetEntry.vulnerabilities.Max(e => e.severity);
                    findings.Add(new VulnerableLibrary(severity, reference.AssemblyName, reference.ProjectsUsedIn));
                }
                else
                {
                    var maxAvailable = nuGetInfo.Where(c => c.IsBeta == false).Max(i => i.CalculatedVersion);

                    if (maxAvailable > nuGetEntry.CalculatedVersion)
                    {
                        findings.Add(new OutdatedLibrary(reference.AssemblyName, reference.ProjectsUsedIn));
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new NuGetProcessingError(reference.AssemblyName, ex));
            }
        }

        return findings;
    }
}
