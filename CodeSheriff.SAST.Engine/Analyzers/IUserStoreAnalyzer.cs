using Microsoft.CodeAnalysis;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings.Cryptography;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSheriff.SAST.Engine.Findings.Authentication;

namespace CodeSheriff.SAST.Engine.Analyzers;

internal static class IUserStoreAnalyzer
{
    internal static List<BaseFinding> FindMisconfiguredUserStores(SyntaxNode root)
    {
        var walker = new IUserStoreSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var symbol in walker.IUserStoreClasses)
        {
            try
            {
                if (!symbol.Interfaces.Any(i => i.ToString().StartsWith("Microsoft.AspNetCore.Identity.IUserLockoutStore<")))
                    findings.Add(new IUserStoreMissingIUserLockoutStore(symbol));
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(symbol, ex));
            }
        }

        return findings;
    }
}
