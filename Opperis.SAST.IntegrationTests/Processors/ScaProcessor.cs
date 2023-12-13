using Opperis.SAST.Engine;
using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.SCA;
using Opperis.SCA.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.IntegrationTests.Processors;

internal static class ScaProcessor
{
    internal static List<BaseFinding> GetScaIssues()
    {
        var retVal = new List<BaseFinding>();

        retVal.AddRange(ScaAnalyzer.GetVulnerableNuGetPackages());

        return retVal;
    }
}
