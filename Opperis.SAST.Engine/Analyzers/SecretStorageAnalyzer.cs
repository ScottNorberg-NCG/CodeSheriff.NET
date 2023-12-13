using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.Database;
using Opperis.SAST.Engine.Findings.Secrets;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SAST.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Analyzers
{
    internal static class SecretStorageAnalyzer
    {
        internal static List<BaseFinding> GetStoredSecrets(StringLiteralSyntaxWalker walker, SyntaxNode root, List<GitLeaksRule> rules)
        {
            if (walker.StringLiterals.Count == 0)
                walker.Visit(root);

            var findings = new List<BaseFinding>();

            Parallel.ForEach(walker.StringLiterals, literal =>
            //foreach (var literal in walker.StringLiterals)
            {
                try
                {
                    var value = literal.ToString().Trim('"');

                    if (value.Length >= Globals.MinStringLengthForSecretMatching)
                    {
                        //Parallel.ForEach(rules, rule =>
                        foreach (var rule in rules)
                        {
                            //We get too many false positives with this one, so skip it
                            //TODO: make skipping this configurable
                            if (rule.id != "generic-api-key")
                            {
                                try
                                {
                                    if (Regex.Match(value, rule.regex).Success)
                                    {
                                        var finding = new SecretFound(rule);
                                        finding.RootLocation = new SourceLocation(literal);
                                        findings.Add(finding);
                                    }
                                    else
                                    {
                                        if (literal.Parent.Parent is VariableDeclaratorSyntax variable)
                                        {
                                            var assignment = variable.ToString();

                                            if (Regex.Match(assignment, rule.regex).Success)
                                            {
                                                var finding = new SecretFound(rule);
                                                finding.RootLocation = new SourceLocation(variable);
                                                findings.Add(finding);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Globals.RuntimeErrors.Add(new InvalidRegex(rule.regex, ex));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Globals.RuntimeErrors.Add(new UnknownSingleFindingError(literal, ex));
                }
            });

            return findings;
        }
    }
}
