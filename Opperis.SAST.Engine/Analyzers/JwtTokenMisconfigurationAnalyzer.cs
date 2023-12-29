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
using Opperis.SCA.Engine.NVD;
using Opperis.SAST.Engine.Findings.Authentication;

namespace Opperis.SAST.Engine.Analyzers;

internal static class JwtTokenMisconfigurationAnalyzer
{
    internal static List<BaseFinding> FindMisconfigurations(SyntaxNode root)
    {
        var walker = new JwtTokenParameterSetSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var param in walker.TokenValidationParameters)
        {
            try
            {
                //TODO: Do we need to look for things beyond just literals?
                if (param.Right.Kind().ToString() == "FalseLiteralExpression")
                {
                    if (param.Left.ToString() == "RequireExpirationTime")
                    {
                        findings.Add(new JwtMissingExpirationTime(param));
                    }
                    if (param.Left.ToString() == "ValidateLifetime")
                    {
                        findings.Add(new JwtWithoutLifetimeValidation(param));
                    }
                    if (param.Left.ToString() == "RequireSignedTokens")
                    {
                        findings.Add(new JwtTokenSigningNotRequired(param));
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(param, ex));
            }
        }

        return findings;
    }
}
