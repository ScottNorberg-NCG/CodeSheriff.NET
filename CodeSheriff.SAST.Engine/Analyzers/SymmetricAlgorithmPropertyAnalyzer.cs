using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Cryptography;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class SymmetricAlgorithmPropertyAnalyzer
{
    public static List<BaseFinding> GetMisconfiguredProperties(SyntaxNode root)
    { 
        var findings = new List<BaseFinding>();
        var walker = new SymmetricCryptographyPropertySyntaxWalker();

        findings.AddRange(FindHardCodedKeys(walker, root));
        findings.AddRange(FindHardCodedIVs(walker, root));
        findings.AddRange(FindECBUses(walker, root));

        return findings;
    }

    internal static List<BaseFinding> FindHardCodedKeys(SymmetricCryptographyPropertySyntaxWalker walker, SyntaxNode root)
    {
        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        //Code is virtually identical to the IV equivalent
        //This is a candidate for refactoring
        //Copied and pasted here because I didn't think of an elegant way of handling the different ways of handling call stacks
        //  depending on where the finding was located
        foreach (var access in walker.CryptoKeySets)
        {
            try
            {
                var parent = access.Parent as AssignmentExpressionSyntax;

                if (parent.Right is IdentifierNameSyntax name)
                {
                    var asSymbol = name.ToSymbol();
                    var definition = SymbolFinder.FindSourceDefinitionAsync(asSymbol, Globals.Solution).Result;

                    var node = root.FindNode(definition.Locations.First().SourceSpan);

                    if (node is VariableDeclaratorSyntax variable)
                    {
                        if (variable.Initializer != null && variable.Initializer.Value is ArrayCreationExpressionSyntax array)
                        {
                            var finding = new HardCodedCryptographyKey();
                            finding.RootLocation = new SourceLocation(parent.Right);

                            var callStack = new CallStack();
                            callStack.AddLocation(parent.Right);
                            callStack.AddLocation(array);
                            finding.CallStacks.Add(callStack);

                            finding.RedactAllByteArrays();

                            findings.Add(finding);
                        }
                        else
                        {
                            //TBD
                        }
                    }
                }
                else if (parent.Right is ArrayCreationExpressionSyntax array)
                {
                    var finding = new HardCodedCryptographyKey();
                    finding.RootLocation = new SourceLocation(parent);
                    finding.RedactAllByteArrays();
                    findings.Add(finding);
                }
                else
                {
                    //Do nothing, may still be hard-coded via a method, but we'll handle that possibility later
                }
            }
            catch (Exception ex) 
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(access, ex));
            }
        }

        return findings;
    }

    internal static List<BaseFinding> FindHardCodedIVs(SymmetricCryptographyPropertySyntaxWalker walker, SyntaxNode root)
    {
        if (walker.CryptoIVSets.Count == 0)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var access in walker.CryptoIVSets)
        {
            try
            {
                var parent = access.Parent as AssignmentExpressionSyntax;

                if (parent.Right != null && parent.Right is IdentifierNameSyntax name)
                {
                    var asSymbol = name.ToSymbol();
                    var definition = SymbolFinder.FindSourceDefinitionAsync(asSymbol, Globals.Solution).Result;

                    var node = root.FindNode(definition.Locations.First().SourceSpan);

                    if (node is VariableDeclaratorSyntax variable)
                    {
                        if (variable.Initializer != null && variable.Initializer.Value is ArrayCreationExpressionSyntax array)
                        {
                            var finding = new HardCodedCryptographyIV();
                            finding.RootLocation = new SourceLocation(parent.Right);

                            var callStack = new CallStack();
                            callStack.AddLocation(parent.Right);
                            callStack.AddLocation(array);
                            finding.CallStacks.Add(callStack);

                            finding.RedactAllByteArrays();

                            findings.Add(finding);
                        }
                        else
                        {
                            //TBD
                        }
                    }
                }
                else if (parent.Right is ArrayCreationExpressionSyntax array)
                {
                    var finding = new HardCodedCryptographyIV();
                    finding.RootLocation = new SourceLocation(parent);
                    finding.RedactAllByteArrays();
                    findings.Add(finding);
                }
                else
                {
                    //Do nothing, may still be hard-coded via a method, but we'll handle that possibility later
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(access, ex));
            }
        }

        return findings;
    }

    internal static List<BaseFinding> FindECBUses(SymmetricCryptographyPropertySyntaxWalker walker, SyntaxNode root)
    {
        if (walker.CryptoModeSets.Count == 0)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var access in walker.CryptoModeSets)
        {
            try
            {
                var parent = access.Parent as AssignmentExpressionSyntax;

                if (parent.Right is MemberAccessExpressionSyntax member)
                {
                    if (member.GetText().ToString() == "CipherMode.ECB")
                    {
                        var finding = new UseOfECBMode();
                        finding.RootLocation = new SourceLocation(member);
                        findings.Add(finding);
                    }
                }
                else
                {
                    //Probably nothing to do
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(access, ex));
            }
        }

        return findings;
    }
}
