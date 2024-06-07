using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Cryptography;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class RSAKeySizeInConstructorAnalyzer
{
    public static List<BaseFinding> FindInadequateKeyLengths(SyntaxNode root)
    {
        var walker = new RSAConstructorSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var rsa in walker.RSAConstructors)
        {
            try
            {
                if (rsa.ArgumentList.Arguments.Count > 0)
                {
                    var first = rsa.ArgumentList.Arguments[0];

                    if (first.Expression.Kind().ToString() == "NumericLiteralExpression")
                    {
                        var value = Convert.ToInt32((first.Expression as LiteralExpressionSyntax).Token.Value);

                        if (value < 2048)
                            findings.Add(new RSAWithInadequateKeyLength(rsa));
                    }
                    else if (first.Expression is IdentifierNameSyntax id)
                    {
                        var value = id.GetLiteralValue<int>();

                        if (value > 0 && value < 2048)
                            findings.Add(new RSAWithInadequateKeyLength(rsa));
                    }
                }
                //TODO: Handle an initializer
                else if (rsa.Initializer != null)
                {
                    foreach (var expr in rsa.Initializer.Expressions)
                    {
                        if (expr is AssignmentExpressionSyntax assignment)
                        {
                            if (assignment.Left is IdentifierNameSyntax leftID)
                            {
                                if (leftID.Identifier.Text == "KeySize")
                                {
                                    if (assignment.Right is LiteralExpressionSyntax literal)
                                    {
                                        var value = literal.GetLiteralValue<int>();

                                        if (value < 2048)
                                            findings.Add(new RSAWithInadequateKeyLength(rsa));
                                    }
                                    else if (assignment.Right is IdentifierNameSyntax rightID)
                                    {
                                        var value = rightID.GetLiteralValue<int>();

                                        if (value < 2048)
                                            findings.Add(new RSAWithInadequateKeyLength(rsa));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(rsa, ex));
            }
        }

        return findings;
    }
}
