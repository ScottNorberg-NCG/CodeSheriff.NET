using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.Secrets;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers;

internal static class TrufflehogAnalyzer
{
    internal static List<BaseFinding> RunTrufflehogScan(string filePath)
    {
        var findings = new List<BaseFinding>();

        var loader = new TrufflehogLoader();
        var results = loader.LoadForFile(filePath);

        if (results.Count > 0)
        {
            foreach (var result in results)
            {
                findings.Add(new TrufflehogSecret(result));
            }
        }

        return findings;
    }
}
