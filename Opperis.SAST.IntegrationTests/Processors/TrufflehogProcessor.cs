using Microsoft.CodeAnalysis;
using CodeSheriff.SAST.Engine;
using CodeSheriff.SAST.Engine.Analyzers;
using CodeSheriff.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.IntegrationTests.Processors;

internal static class TrufflehogProcessor
{
    internal static List<BaseFinding> ProcessSolution()
    {
        var findings = new List<BaseFinding>();

        var totalDocuments = Globals.Solution.Projects.SelectMany(p => p.AdditionalDocuments).Count() + Globals.Solution.Projects.SelectMany(p => p.Documents).Count();

        var currentIndex = 1;

        foreach (var project in Globals.Solution.Projects)
        {
            foreach (var doc in project.AdditionalDocuments)
            {
                findings.AddRange(TrufflehogAnalyzer.RunTrufflehogScan(doc.FilePath));
                Console.WriteLine($"Processed {currentIndex} of {totalDocuments}");
                currentIndex++;
            }

            foreach (var doc in project.Documents)
            {
                findings.AddRange(TrufflehogAnalyzer.RunTrufflehogScan(doc.FilePath));
                Console.WriteLine($"Processed {currentIndex} of {totalDocuments}");
                currentIndex++;
            }
        }

        return findings;
    }
}
