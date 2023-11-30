﻿using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SAST.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.IntegrationTests.Processors
{
    internal static class FileManipulationProcessor
    {
        internal static List<BaseFinding> GetFileManipulations()
        {
            var retVal = new List<BaseFinding>();

            foreach (var project in Globals.Solution.Projects)
            {
                Globals.Compilation = project.GetCompilationAsync().Result;

                foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
                {
                    var root = syntaxTree.GetRoot();

                    var walker = new FileManipulationSyntaxWalker();
                    retVal.AddRange(FileManipulationAnalyzer.FindFileManipulations(walker, root));
                }
            }

            return retVal;
        }
    }
}