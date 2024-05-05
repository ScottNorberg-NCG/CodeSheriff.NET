using CodeSheriff.SAST.Engine;
using CodeSheriff.SAST.Engine.Analyzers;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.SCA;
using CodeSheriff.SCA.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.IntegrationTests.Processors;

internal static class ScaProcessor
{
    internal static List<BaseFinding> GetScaIssues()
    {
        var retVal = new List<BaseFinding>();

        retVal.AddRange(ScaAnalyzer.GetVulnerableNuGetPackages());

        return retVal;
    }
}
