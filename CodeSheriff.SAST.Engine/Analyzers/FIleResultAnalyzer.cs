﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Database;
using CodeSheriff.SAST.Engine.Findings.Resources;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using CodeSheriff.SCA.Engine.NVD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class FileResultAnalyzer
{
    public static List<BaseFinding> GetFileResults(SyntaxNode root)
    {
        var walker = new FileResultSyntaxWalker();

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
