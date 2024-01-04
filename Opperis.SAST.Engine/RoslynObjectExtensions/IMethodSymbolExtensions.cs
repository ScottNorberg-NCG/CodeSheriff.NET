using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis;
using Opperis.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions;

internal static class IMethodSymbolExtensions
{
    internal static MethodDeclarationSyntax ToMethodDeclarationSyntax(this IMethodSymbol symbol)
    { 
        return symbol.DeclaringSyntaxReferences.First().GetSyntax() as MethodDeclarationSyntax;
    }

    internal static bool IsUIProcessor(this IMethodSymbol symbol)
    {
        var method = symbol.ToMethodDeclarationSyntax();

        if (method == null)
            return false;

        return method.IsUIProcessor();
    }

    internal static List<string>? GetRoles(this IMethodSymbol symbol)
    {
        List<string>? roles = null;
        
        var allowAnonymousAttributes = symbol.GetAttributes().Where(a => a.InheritsFrom("Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute"));

        if (allowAnonymousAttributes.Any())
        {
            return null;
        }
        else
        {
            var authorizeAttributes = symbol.GetAttributes().Where(a => a.InheritsFrom("Microsoft.AspNetCore.Authorization.AuthorizeAttribute"));

            if (authorizeAttributes.Any())
            {
                roles = new List<string>();

                foreach (var attribute in authorizeAttributes)
                {
                    var roleParameter = attribute.NamedArguments.SingleOrDefault(a => a.Key == "Roles");

                    var roleAsString = roleParameter.Value.Value.ToString();

                    roles.AddRange(roleAsString.Split(",").Select(s => s.Trim()));
                }
            }
            else
            {
                var asExpression = symbol.ToMethodDeclarationSyntax();
                if (asExpression != null)
                {
                    var parentClass = asExpression.Parent as ClassDeclarationSyntax;

                    if (parentClass != null)
                    {
                        var model = Globals.SearchForSemanticModel(parentClass.SyntaxTree);
                        var parentAttributes = parentClass.AttributeLists.SelectMany(l => l.Attributes).Where(a => a.IsOfType("Microsoft.AspNetCore.Authorization.AuthorizeAttribute", model));

                        if (parentAttributes.Any())
                        {
                            roles = new List<string>();

                            foreach (var attribute in parentAttributes)
                            {
                                var roleParameter = attribute.ArgumentList.Arguments.SingleOrDefault(a => a.ToString() == "Roles");

                                if (roleParameter != null)
                                {
                                    int i = 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        return roles;
    }

    internal static List<CallStack> CrawlTrees(this IMethodSymbol method, CallStack stack)
    {
        var callStacks = new List<CallStack>();

        if (method.IsTestMethod())
        { 
            callStacks.Add(stack);
            return callStacks;
        }

        var references = method.GetMethodsReferencedIn();

        if (!stack.Locations.Any(l => l.Equals(new SourceLocation(method))))
        {
            if (!stack.AddLocation(method))
            {
                callStacks.Add(stack);
                return callStacks;
            }
        } 
        else
        {
            callStacks.Add(stack);

            //We could be in a recursive loop, so just return now
            return callStacks;
        }
        //else
        //    throw new InvalidOperationException("SourceLocation was already added. Track this down so we can avoid infinite loops due to recursion");

        if (references.Count == 0)
        {
            stack.AddLocation(method.ContainingSymbol);
            callStacks.Add(stack);
        }
        else if (references.Count == 1)
        {
            foreach (var callStack in CrawlTrees(references.Single(), stack))
            {
                callStacks.Add(callStack);

                if (callStacks.Count >= Globals.MaxCallStackCount)
                    break;
            }
        }
        else
        {
            foreach (var reference in references)
            {
                //If we already have the location in the callstack we might be stuck in a recursive loop
                //So just return the current callstack to avoid stackoverflow exceptions
                if (stack.Locations.Any(l => l.Equals(new SourceLocation(reference))))
                {
                    callStacks.Add(stack);
                }
                else
                {
                    foreach (var callStack in CrawlTrees(reference, stack.Clone()))
                    {
                        callStacks.Add(callStack);

                        if (callStacks.Count >= Globals.MaxCallStackCount)
                            break;
                    }
                }

                if (callStacks.Count >= Globals.MaxCallStackCount)
                    break;
            }
        }

        return callStacks;
    }

    internal static List<IMethodSymbol> GetMethodsReferencedIn(this IMethodSymbol methodSymbol)
    {
        var toReturn = new List<IMethodSymbol>();

        var references = SymbolFinder.FindReferencesAsync(methodSymbol, Globals.Solution).Result;

        foreach (var reference in references)
        {
            foreach (var referenceLocation in reference.Locations)
            {
                //var referenceDocument = Globals.Solution.GetDocument(referenceLocation.Document.Id);
                //var referenceRoot = referenceDocument.GetSyntaxRootAsync().Result;

                var referenceRoot = referenceLocation.Document.GetSyntaxRootAsync().Result;

                // Find the syntax node where the method is called
                var referenceNode = referenceRoot.FindNode(referenceLocation.Location.SourceSpan);

                SyntaxNode? parent = referenceNode.Parent;
                
                while (parent != null && !(parent is MethodDeclarationSyntax))
                {
                    parent = parent.Parent;
                }

                if (parent != null) 
                {
                    var asMethod = parent as MethodDeclarationSyntax;

                    if (!asMethod.IsTestMethod())
                    {
                        try
                        {
                            var semanticModel = Globals.SearchForSemanticModel(parent.SyntaxTree);
                            var symbol = (IMethodSymbol)semanticModel.GetDeclaredSymbol(parent);

                            if (!toReturn.Any(s => s.ToString() == symbol.ToString()))
                                toReturn.Add(symbol);
                        }
                        catch (ArgumentException)
                        {
                            //Likely due to parent.SyntaxTree being outside of the compilation
                            //That means the source code is not a part of the solution and is out of scope for the scan
                            //TODO: Determine if this can cause unanticipated problems
                        }
                    }
                }
            }
        }

        return toReturn;
    }

    internal static bool IsTestMethod(this IMethodSymbol symbol)
    {
        var location = symbol.Locations.First();
        var syntaxTree = location.SourceTree;
        var syntaxTreeRoot = syntaxTree.GetRoot();
        var syntaxNode = syntaxTreeRoot.FindNode(location.SourceSpan);

        if (syntaxNode is MethodDeclarationSyntax method)
        {
            var model = Globals.SearchForSemanticModel(method.SyntaxTree);

            foreach (var list in method.AttributeLists)
            {
                foreach (var attribute in list.Attributes)
                {
                    var attributeType = model.GetTypeInfo(attribute).Type.ToString();

                    if (attributeType.StartsWith("Xunit."))
                        return true;
                    else if (attributeType.StartsWith("Microsoft.VisualStudio.TestTools.UnitTesting."))
                        return true;
                }
            }
        }
        //Probably won't happen since constructors for unit tests do not have parameters, but keep it in here so we don't have to add it back in later
        else if (syntaxNode is ConstructorDeclarationSyntax constructor) 
        {
            var model = Globals.SearchForSemanticModel(constructor.SyntaxTree);

            foreach (var list in constructor.AttributeLists)
            {
                foreach (var attribute in list.Attributes)
                {
                    var attributeType = model.GetTypeInfo(attribute).Type.ToString();

                    if (attributeType.StartsWith("Xunit."))
                        return true;
                    else if (attributeType.StartsWith("Microsoft.VisualStudio.TestTools.UnitTesting."))
                        return true;
                }
            }
        }

        return false;
    }
}
