﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Http;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class CookieConfigurationAnalyzer
{
    public static List<BaseFinding> FindMisconfiguredCookies(SyntaxNode root)
    {
        var walker = new CookieAppendSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var cookie in walker.MethodCalls) 
        {
            try
            {
                var cookieOptions = cookie.ArgumentList.Arguments.SingleOrDefault(a => a.IsOfType("Microsoft.AspNetCore.Http.CookieOptions"));

                if (cookieOptions == null)
                {
                    var finding = new CookieAddedWithoutOptions(cookie);
                    findings.Add(finding);
                }
                else if (cookieOptions.Expression is ObjectCreationExpressionSyntax)
                {
                    var finding = new CookieAddedWithDefaultOptions(cookie);
                    findings.Add(finding);
                }
                else if (cookieOptions.Expression is IdentifierNameSyntax id)
                {
                    var idAsSymbol = id.ToSymbol();

                    bool secureIsSet = false;
                    //Don't bother with SameSite right now
                    //bool sameSiteIsSet = false;
                    bool httpOnlyIsSet = false;

                    foreach (var reference in idAsSymbol.GetReferenceExpressions())
                    {
                        MemberAccessExpressionSyntax parent;

                        if (reference is MemberAccessExpressionSyntax)
                            parent = (MemberAccessExpressionSyntax)reference;
                        else
                            parent = reference.Parent as MemberAccessExpressionSyntax;

                        if (parent == null)
                            continue;

                        if (parent.Parent is AssignmentExpressionSyntax assignment)
                        {
                            var propertyName = parent.Name.ToString();

                            switch (propertyName)
                            {
                                case "Secure":
                                    secureIsSet = true;

                                    if (assignment.Right is LiteralExpressionSyntax secureLiteral)
                                    {
                                        if (secureLiteral.Kind().ToString() == "FalseLiteralExpression")
                                        {
                                            var finding = new CookieAddedWithSecureSetToFalse(parent);
                                            findings.Add(finding);
                                        }
                                    }
                                    break;
                                //Skip this for now - findings here are likely to cause more issues than they fix
                                //case "SameSite":
                                //    sameSiteIsSet = true;

                                //    if (assignment.Right is MemberAccessExpressionSyntax memberAccess)
                                //    {
                                //        int i = 1;
                                //    }
                                //    break;
                                case "HttpOnly":
                                    httpOnlyIsSet = true;

                                    if (assignment.Right is LiteralExpressionSyntax httpOnlyLiteral)
                                    {
                                        if (httpOnlyLiteral.Kind().ToString() == "FalseLiteralExpression")
                                        {
                                            var finding = new CookieAddedWithHttpOnlySetToFalse(parent);
                                            findings.Add(finding);
                                        }
                                    }
                                    break;
                                default:
                                    //Skip
                                    break;
                            }
                        }
                    }
                    if (!secureIsSet)
                    {
                        var finding = new CookieAddedWithSecureNotSet(cookieOptions.Expression);
                        findings.Add(finding);
                    }
                    if (!httpOnlyIsSet)
                    {
                        var finding = new CookieAddedWithHttpOnlyNotSet(cookieOptions.Expression);
                        findings.Add(finding);
                    }
                }
                else
                {
                    //Unknown, ignore for now
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(cookie, ex));
            }
        }

        return findings;
    }
}
