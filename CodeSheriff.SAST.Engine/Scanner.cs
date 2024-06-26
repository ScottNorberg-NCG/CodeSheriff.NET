﻿using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using CodeSheriff.SAST.Engine.Analyzers;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.HtmlTagParsing;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using CodeSheriff.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine
{
    internal static class Scanner
    {
        internal static List<BaseFinding> Scan(string solutionFilePath, bool includeNuGet, bool includeTrufflehog)
        {
            throw new NotImplementedException();

            //if (!MSBuildLocator.IsRegistered)
            //    MSBuildLocator.RegisterDefaults();

            //var findings = new List<BaseFinding>();

            //using (var workspace = MSBuildWorkspace.Create())
            //{
            //    Globals.Solution = workspace.OpenSolutionAsync(solutionFilePath).Result;

            //    findings.AddRange(OverpostingAnalyzer.FindEFObjectsAsParameters());

            //    if (includeTrufflehog)
            //    {
            //        foreach (var project in Globals.Solution.Projects)
            //        {
            //            foreach (var doc in project.AdditionalDocuments)
            //            {
            //                findings.AddRange(TrufflehogAnalyzer.RunTrufflehogScan(doc.FilePath));
            //            }

            //            foreach (var doc in project.Documents)
            //            {
            //                findings.AddRange(TrufflehogAnalyzer.RunTrufflehogScan(doc.FilePath));
            //            }
            //        }
            //    }

            //    List<GitLeaksRule> rules = new List<GitLeaksRule>();
                
            //    //if (includeSecrets)
            //    //    rules = CodeSheriff.SAST.Secrets.RulesEngine.GetGitLeaksRules();

            //    if (includeNuGet)
            //        findings.AddRange(ScaAnalyzer.GetVulnerableNuGetPackages());

            //    foreach (var project in Globals.Solution.Projects)
            //    {
            //        Globals.Compilation = project.GetCompilationAsync().Result;

            //        foreach (var cshtmlFile in project.AdditionalDocuments.Where(d => d.FilePath.EndsWith(".cshtml")))
            //        {
            //            ParseJavaScriptTags(findings, cshtmlFile);
            //            ParseLinkTags(findings, cshtmlFile);
            //            ParseStyleTags(findings, cshtmlFile);
            //        }

            //        foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            //        {
            //            var root = syntaxTree.GetRoot();

            //            var databaseCalls = new DatabaseCommandTextSyntaxWalker();
            //            findings.AddRange(SQLInjectionAnalyzer.GetSQLInjections(databaseCalls, root));

            //            var controllerMethods = new UIProcessorMethodSyntaxWalker();
            //            findings.AddRange(CsrfAnalyzer.FindCsrfIssues(controllerMethods, root));
            //            findings.AddRange(ValueShadowingAnalyzer.FindValueShadowingPossibilities(controllerMethods, root));

            //            var databaseConnections = new DatabaseConnectionOpenSyntaxWalker();
            //            findings.AddRange(DatabaseConnectionOpenAnalyzer.FindUnsafeDatabaseConnectionOpens(databaseConnections, root));

            //            var cryptoKeyFinder = new SymmetricCryptographyPropertySyntaxWalker();
            //            findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindHardCodedKeys(cryptoKeyFinder, root));
            //            findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindHardCodedIVs(cryptoKeyFinder, root));
            //            findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindECBUses(cryptoKeyFinder, root));

            //            var cryptoAlgorithmFinder = new SymmetricAlgorithmSyntaxWalker();
            //            findings.AddRange(SymmetricAlgorithmAnalyzer.FindDeprecatedAlgorithms(cryptoAlgorithmFinder, root));

            //            var externalRedirects = new ExternalRedirectSyntaxWalker();
            //            findings.AddRange(ExternalRedirectAnalyzer.FindProblematicExternalRedirects(externalRedirects, root));

            //            var hardCodedConnectionStrings = new DatabaseConnectionStringSyntaxWalker();
            //            findings.AddRange(HardCodedConnectionStringAnalyzer.FindHardCodedConnectionStrings(hardCodedConnectionStrings, root));

            //            var problematicHtmlRaws = new HtmlRawSyntaxWalker();
            //            var rawAnalyzer = new HtmlRawAnalyzer();
            //            findings.AddRange(rawAnalyzer.FindXssIssues(problematicHtmlRaws, root));

            //            var problematicHtmlHelpers = new HtmlHelperSyntaxWalker();
            //            var helperAnalyzer = new HtmlHelperAnalyzer();
            //            findings.AddRange(helperAnalyzer.FindXssIssues(problematicHtmlHelpers, root));

            //            var overpostingsAsBindObjects = new RazorPageBindObjectSyntaxWalker();
            //            findings.AddRange(OverpostingAnalyzer.FindEFObjectsAsBindObjects(overpostingsAsBindObjects, root));


            //            SearchForCookieManipulations(findings, root);
            //            SearchForFileManipulations(findings, root);

            //            //if (includeSecrets)
            //            //    SearchForSecrets(findings, root, rules);
            //        }
            //    }
            //}

            //return findings;
        }

        //private static void ParseJavaScriptTags(List<BaseFinding> findings, TextDocument? cshtmlFile)
        //{
        //    var scripts = CSHtmlScriptTagParser.GetScriptTags(cshtmlFile.GetTextAsync().Result.ToString());
        //    findings.AddRange(CSHtmlScriptTagParser.ParseJavaScriptFindings(scripts, cshtmlFile));
        //}

        //private static void ParseLinkTags(List<BaseFinding> findings, TextDocument? cshtmlFile)
        //{
        //    var links = CSHtmlLinkTagParser.GetLinkTags(cshtmlFile.GetTextAsync().Result.ToString());
        //    findings.AddRange(CSHtmlLinkTagParser.ParseLinkTagFindings(links, cshtmlFile));
        //}

        //private static void ParseStyleTags(List<BaseFinding> findings, TextDocument? cshtmlFile)
        //{
        //    var styleTags = CSHtmlStyleTagParser.GetStyleTags(cshtmlFile.GetTextAsync().Result.ToString());
        //    findings.AddRange(CSHtmlStyleTagParser.ParseStyleTagFindings(styleTags, cshtmlFile));
        //}

        //private static void SearchForCookieManipulations(List<BaseFinding> findings, SyntaxNode root)
        //{
        //    findings.AddRange(CookieConfigurationAnalyzer.FindMisconfiguredCookies(root));
        //}

        //private static void SearchForFileManipulations(List<BaseFinding> findings, SyntaxNode root)
        //{
        //    findings.AddRange(FileManipulationAnalyzer.FindFileManipulations(root));
        //}

        //private static void SearchForSecrets(List<BaseFinding> findings, SyntaxNode root, List<GitLeaksRule> rules)
        //{
        //    var stringLiteralWalker = new StringLiteralSyntaxWalker();
        //    findings.AddRange(SecretStorageAnalyzer.GetStoredSecrets(stringLiteralWalker, root, rules));
        //}
    }
}
