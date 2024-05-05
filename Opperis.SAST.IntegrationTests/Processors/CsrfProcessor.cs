﻿using CodeSheriff.SAST.Engine.Analyzers;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using CodeSheriff.SAST.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.IntegrationTests.Processors;

internal static class CsrfProcessor
{
    internal static List<BaseFinding> GetCsrfIssues()
    {
        return CsrfAnalyzer.FindCsrfIssues();
    }
}
