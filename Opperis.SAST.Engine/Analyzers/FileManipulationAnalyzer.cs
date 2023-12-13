using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.Findings.Resources;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using Opperis.SAST.Engine.SyntaxWalkers;

namespace Opperis.SAST.Engine.Analyzers;

internal static class FileManipulationAnalyzer
{
    internal static List<BaseFinding> FindFileManipulations(FileManipulationSyntaxWalker walker, SyntaxNode root)
    {
        if (!walker.FileManipulations.Any())
            walker.Visit(root);

        var findings = new List<BaseFinding>();

        foreach (var call in walker.FileManipulations)
        {
            try
            {
                var methodName = ((MemberAccessExpressionSyntax)call.Expression).Name.Identifier.Text;
                if (methodName.StartsWith("Append") || methodName.StartsWith("Create") || methodName.StartsWith("Open") ||
                            methodName.StartsWith("Read") || methodName.StartsWith("Write") || methodName.StartsWith("Delete"))
                {
                    var pathArgument = call.ArgumentList.Arguments[0].Expression;

                    var callStacks = pathArgument.GetCallStacks();

                    if (callStacks.SelectMany(cs => cs.Locations).Where(l => l.Symbol is IMethodSymbol).Select(m => m.Symbol as IMethodSymbol).Any(m => m.IsUIProcessor()))
                    //if (callStacks.Any())
                    {
                        BaseFinding finding;

                        if (methodName.StartsWith("Append") || methodName.StartsWith("Write"))
                            finding = new UnvalidatedFilePathForWrite();
                        else if (methodName.StartsWith("Create"))
                            finding = new UnvalidatedFilePathForCreate();
                        else if (methodName.StartsWith("Delete"))
                            finding = new UnvalidatedFilePathForDelete();
                        else if (methodName.StartsWith("Open") || methodName.StartsWith("Read"))
                            finding = new UnvalidatedFilePathForRead();
                        else
                            throw new NotImplementedException($"Cannot find appropriate finding name for {methodName}");

                        finding.RootLocation = new SourceLocation(pathArgument);

                        foreach (var cs in callStacks) 
                        { 
                            if (cs.Locations.Where(l => l.Symbol is IMethodSymbol).Select(m => m.Symbol as IMethodSymbol).Any(m => m.IsUIProcessor()))
                                finding.CallStacks.Add(cs);
                        }

                        findings.Add(finding);
                    }
                }
                else if (methodName.StartsWith("Move"))
                {
                    foreach (var arg in call.ArgumentList.Arguments)
                    {
                        var callStacks = arg.Expression.GetCallStacks();

                        if (callStacks.Any())
                        {
                            BaseFinding finding = new UnvalidatedFilePathForMove();

                            finding.RootLocation = new SourceLocation(arg.Expression);

                            foreach (var cs in callStacks)
                            {
                                finding.CallStacks.Add(cs);
                            }

                            findings.Add(finding);
                        }
                    }
                }
                else
                    throw new NotImplementedException($"Cannot find appropriate finding name for {methodName}");
            }
            catch (Exception ex) 
            {
                Globals.RuntimeErrors.Add(new UnknownSingleFindingError(call, ex));
            }
        }

        return findings;
    }
}
