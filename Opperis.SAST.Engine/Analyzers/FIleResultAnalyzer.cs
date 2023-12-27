using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.Database;
using Opperis.SAST.Engine.Findings.Resources;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SCA.Engine.NVD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers;

internal static class FileResultAnalyzer
{
    internal static List<BaseFinding> GetFileResults(FileResultSyntaxWalker walker, SyntaxNode root)
    {
        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var result in walker.FileResults)
        {
            try
            {
                var arg1 = result.ArgumentList.Arguments.First().Expression;

                var callStacks = arg1.GetCallStacks();

                if (callStacks.Count > 0)
                {
                    var countExternal = callStacks.SelectMany(cs => cs.Locations).Where(l => l.Symbol is IMethodSymbol).Select(m => m.Symbol as IMethodSymbol).Count(m => m.IsUIProcessor());

                    if (countExternal > 0)
                    {
                        BaseFinding finding;

                        var type = result.GetUnderlyingType();

                        if (type != null)
                        {
                            var typeString = type.ToString().Replace("?", "");

                            if (typeString == "Microsoft.AspNetCore.Mvc.PhysicalFileResult")
                                finding = new UnprotectedPhysicalFileResultPath();
                            else if (typeString == "Microsoft.AspNetCore.Mvc.VirtualFileResult")
                                finding = new UnprotectedVirtualFileResultPath();
                            else
                                throw new NotImplementedException($"Could not find FileResult type for {typeString}");

                            finding.RootLocation = new SourceLocation(arg1);

                            foreach (var cs in callStacks)
                            {
                                finding.CallStacks.Add(cs);
                            }

                            findings.Add(finding);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(result, ex));
            }
        }

        return findings;
    }
}
