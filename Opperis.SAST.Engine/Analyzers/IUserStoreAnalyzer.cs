using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings.Cryptography;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opperis.SAST.Engine.Findings.Authentication;

namespace Opperis.SAST.Engine.Analyzers;

internal static class IUserStoreAnalyzer
{
    internal static List<BaseFinding> FindMisconfiguredUserStores(IUserStoreSyntaxWalker walker, SyntaxNode root)
    {
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
