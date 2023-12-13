using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.Database;
using Opperis.SAST.Engine.SyntaxWalkers;

namespace Opperis.SAST.Engine.Analyzers;

internal static class HardCodedConnectionStringAnalyzer
{
    internal static List<BaseFinding> FindHardCodedConnectionStrings(DatabaseConnectionStringSyntaxWalker walker, SyntaxNode root)
    {
        if (walker.ConnectionStringSets.Count == 0 && walker.NewConnectionStrings.Count == 0)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var connectionString in walker.ConnectionStringSets)
        {
            try
            {
                if (connectionString.Parent is AssignmentExpressionSyntax assignment)
                {
                    if (assignment.Right is LiteralExpressionSyntax literal)
                    {
                        BaseFinding finding;

                        if (literal.ToString().Contains("Password"))
                            finding = new HardCodedConnectionStringWithPassword();
                        else
                            finding = new HardCodedConnectionStringWithoutPassword();

                        finding.RootLocation = new SourceLocation(assignment);
                        findings.Add(finding);
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(connectionString, ex));
            }
        }

        foreach (var constructor in walker.NewConnectionStrings)
        {
            if (constructor.ArgumentList.Arguments.Count > 0)
            {
                var argValue = constructor.ArgumentList.Arguments.First().Expression;

                if (argValue is LiteralExpressionSyntax literal)
                {
                    BaseFinding finding;

                    if (literal.ToString().Contains("Password"))
                        finding = new HardCodedConnectionStringWithPassword();
                    else
                        finding = new HardCodedConnectionStringWithoutPassword();

                    finding.RootLocation = new SourceLocation(constructor);
                    findings.Add(finding);
                }
            } 
        }

        return findings;
    }

}
