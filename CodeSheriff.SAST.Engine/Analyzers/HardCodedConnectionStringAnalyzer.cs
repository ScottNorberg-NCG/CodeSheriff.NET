using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Database;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class HardCodedConnectionStringAnalyzer
{
    public static List<BaseFinding> FindHardCodedConnectionStrings(SyntaxNode root)
    {
        var walker = new DatabaseConnectionStringSyntaxWalker();

        if (!walker.HasRun)
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
                        AddFindingForLiteral(findings, assignment, literal);
                    }
                    else if (assignment.Right is IdentifierNameSyntax id)
                    {
                        var definition = id.GetDefinitionNode(id.Ancestors().Last());

                        if (definition != null)
                        {
                            //Check to see if we have two nodes. If so, this should be an EqualsValueClauseSyntax and a LiteralExpressionSyntax
                            if (definition.DescendantNodes().Count() == 2)
                            {
                                AddFindingForVariable(findings, id, definition);
                            }
                        }
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
                else if (argValue is IdentifierNameSyntax id)
                {
                    var definition = id.GetDefinitionNode(id.Ancestors().Last());

                    if (definition != null)
                    {
                        //Check to see if we have two nodes. If so, this should be an EqualsValueClauseSyntax and a LiteralExpressionSyntax
                        if (definition.DescendantNodes().Count() == 2)
                        {
                            AddFindingForVariable(findings, id, definition);
                        }
                    }
                }
            } 
        }

        return findings;
    }

    private static void AddFindingForLiteral(List<BaseFinding> findings, AssignmentExpressionSyntax assignment, LiteralExpressionSyntax literal)
    {
        BaseFinding finding;

        if (literal.ToString().Contains("Password"))
            finding = new HardCodedConnectionStringWithPassword();
        else
            finding = new HardCodedConnectionStringWithoutPassword();

        finding.RootLocation = new SourceLocation(assignment);
        findings.Add(finding);
    }

    private static void AddFindingForVariable(List<BaseFinding> findings, IdentifierNameSyntax id, SyntaxNode? definition)
    {
        var variable = definition.DescendantNodes().SingleOrDefault(n => n is LiteralExpressionSyntax) as LiteralExpressionSyntax;

        BaseFinding finding;

        if (variable.ToString().Contains("Password"))
            finding = new HardCodedConnectionStringWithPassword();
        else
            finding = new HardCodedConnectionStringWithoutPassword();

        finding.RootLocation = new SourceLocation(variable);

        var callStack = new CallStack();

        callStack.AddLocation(variable);
        callStack.AddLocation(id);

        findings.Add(finding);
    }
}
