using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Cryptography;
using CodeSheriff.SAST.Engine.Findings.InputValidation;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Analyzers;

public static class ModelValidationAnalyzer
{
    public static List<BaseFinding> FindMissingModelValidations(SyntaxNode root)
    {
        var walker = new UIProcessorMethodSyntaxWalker();

        if (!walker.HasRun)
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var method in walker.ControllerMethods)
        {
            try
            {
                if (method.ParameterList.Parameters.Any(p => p.Type is IdentifierNameSyntax))
                {
                    var modelStateWalker = new ModelStateIsValidSyntaxWalker();
                    modelStateWalker.Visit(method);

                    if (!modelStateWalker.ModelStateIsValidChecks.Any())
                    {
                        var finding = new ControllerMethodMissingCallToIsValid(method);
                        findings.Add(finding);
                    }

                    var model = Globals.Compilation.GetSemanticModel(method.SyntaxTree);
                    foreach (var parameter in method.ParameterList.Parameters.Where(p => p.Type is IdentifierNameSyntax).Select(p => p.Type as IdentifierNameSyntax))
                    {
                        var typeSymbol = model.GetTypeInfo(parameter).Type;

                        if (!typeSymbol.GetMembers().Where(m => m is IPropertySymbol).Select(m => m as IPropertySymbol).Any(p => p.HasValidatorAttribute()))
                        {
                            var finding = new ControllerBinderMissingValidators(method);
                            findings.Add(finding);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(method, ex));
            }
        }

        foreach (var method in walker.RazorPageMethods)
        {
            try
            {
                var bindObjectWalker = new BindObjectSyntaxWalker();
                bindObjectWalker.Visit(method);

                if (bindObjectWalker.BindObjectReferences.Any())
                {
                    var modelStateWalker = new ModelStateIsValidSyntaxWalker();
                    modelStateWalker.Visit(method);

                    if (!modelStateWalker.ModelStateIsValidChecks.Any())
                    {
                        var finding = new RazorPageMethodMissingCallToIsValid(method);
                        findings.Add(finding);
                    }

                    foreach (var bindObject in bindObjectWalker.BindObjectReferences)
                    {
                        if (!bindObject.AsType.GetMembers().Where(m => m is IPropertySymbol).Select(m => m as IPropertySymbol).Any(p => p.HasValidatorAttribute()))
                        {
                            var finding = new RazorPageBindObjectMissingValidators(method);
                            findings.Add(finding);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(method, ex));
            }
        }

        return findings;
    }
}
