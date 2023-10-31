﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.Database;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers
{
    internal static class DatabaseConnectionOpenAnalyzer
    {
        internal static List<BaseFinding> FindUnsafeDatabaseConnectionOpens(DatabaseConnectionOpenSyntaxWalker walker, SyntaxNode root)
        {
            if (walker.ConnectionOpens.Count == 0)
                walker.Visit(root);

            var findings = new List<BaseFinding>();

            foreach (var open in walker.ConnectionOpens)
            {
                var parentMethod = open.Ancestors().OfType<MethodDeclarationSyntax>().First();
                var dbCloseSyntaxWalker = new DatabaseConnectionCloseSyntaxWalker();
                dbCloseSyntaxWalker.Visit(parentMethod);

                //Not *really* safe, since we might have multiple connection objects that are opened but only one closed
                //This should be good enough for now until a better solution is found
                if (!dbCloseSyntaxWalker.ConnectionCloses.Any())
                {
                    var finding = new SqlConnectionNotClosed();
                    finding.RootLocation = new SourceLocation(open);
                    findings.Add(finding);
                }
                else
                {
                    foreach (var close in dbCloseSyntaxWalker.ConnectionCloses)
                    {
                        if (!close.Ancestors().OfType<FinallyClauseSyntax>().Any())
                        {
                            var finding = new SqlConnectionNotClosedInTryFinally();
                            finding.RootLocation = new SourceLocation(open);
                            findings.Add(finding);
                        }
                    }
                }
            }

            return findings;
        }
    }
}