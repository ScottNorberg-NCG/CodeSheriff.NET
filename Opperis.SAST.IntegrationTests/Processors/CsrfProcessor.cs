﻿using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SAST.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.IntegrationTests.Processors;

internal static class CsrfProcessor
{
    internal static List<BaseFinding> GetCsrfIssues()
    {
        return CsrfAnalyzer.FindCsrfIssues();
    }
}
