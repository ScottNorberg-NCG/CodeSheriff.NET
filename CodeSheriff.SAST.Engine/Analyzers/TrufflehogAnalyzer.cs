using Microsoft.CodeAnalysis;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Secrets;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class TrufflehogAnalyzer
{
    public static List<BaseFinding> RunTrufflehogScan(string filePath)
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
