﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Database;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class SQLInjectionViaEntityFrameworkAnalyzer
{
    public static List<BaseFinding> GetSQLInjections(SyntaxNode root)
    {
        var walker = new EntityFrameworkDbCallSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var method in walker.DatabaseCalls)
        {
            try
            {
                if (method.ArgumentList.Arguments.Count() == 1)
                { 
                    var arg = method.ArgumentList.Arguments[0];
                    if (!(arg.Expression is LiteralExpressionSyntax))
                    {
                        var callStacks = arg.Expression.GetCallStacks();

                        if (callStacks.Count > 0)
                        {
                            var countExternal = callStacks.SelectMany(cs => cs.Locations).Where(l => l.Symbol is IMethodSymbol).Select(m => m.Symbol as IMethodSymbol).Count(m => m.IsUIProcessor());

                            BaseFinding finding;

                            //This is a bit simplified - it only looks to see if the call stack ends in a publicly-accessible method
                            //(like a controller class)
                            //It does not look at individual properties
                            //TODO: look at individual properties
                            if (countExternal > 0)
                                finding = new SqlInjection_DataFromView();
                            else
                                finding = new SqlInjection_DataFromOther();

                            finding.RootLocation = new SourceLocation(arg);

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
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(method, ex));
            }
        }

        return findings;
    }
}
