using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Database;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class DatabaseConnectionOpenAnalyzer
{
    public static List<BaseFinding> FindUnsafeDatabaseConnectionOpens(SyntaxNode root)
    {
        var walker = new DatabaseConnectionOpenSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var open in walker.ConnectionOpens)
        {
            try
            {
                var parentMethod = open.Ancestors().OfType<MethodDeclarationSyntax>().First();
                var dbCloseSyntaxWalker = new DatabaseConnectionCloseSyntaxWalker();
                dbCloseSyntaxWalker.Visit(parentMethod);

                //Not *really* safe, since we might have multiple connection objects that are opened but only one closed
                //This should be good enough for now until a better solution is found
                if (!dbCloseSyntaxWalker.MethodCalls.Any())
                {
                    var finding = new SqlConnectionNotClosed();
                    finding.RootLocation = new SourceLocation(open);
                    findings.Add(finding);
                }
                else
                {
                    foreach (var close in dbCloseSyntaxWalker.MethodCalls)
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
            catch (Exception ex)
            { 
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(open, ex));
            }
        }

        return findings;
    }
}
