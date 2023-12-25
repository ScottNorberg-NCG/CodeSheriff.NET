using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class ExpressionSyntaxExtensions
    {
        internal static ISymbol ToSymbol(this ExpressionSyntax expression)
        {
            SemanticModel model;
            
            if (Globals.Compilation.ContainsSyntaxTree(expression.SyntaxTree))
                model = Globals.Compilation.GetSemanticModel(expression.SyntaxTree);
            else
                model = Globals.SearchForSemanticModel(expression.SyntaxTree);

            return model.GetSymbolInfo(expression).Symbol;
        }

        internal static SyntaxNode? GetDefinitionNode(this ExpressionSyntax expression, SyntaxNode root)
        {
            var asSymbol = expression.ToSymbol();
            var definition = SymbolFinder.FindSourceDefinitionAsync(asSymbol, Globals.Solution).Result;

            if (definition == null)
                return null;

            try
            {
                return root.FindNode(definition.Locations.First().SourceSpan);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        internal static ITypeSymbol? GetUnderlyingType(this ExpressionSyntax expression)
        {
            if (expression is LiteralExpressionSyntax)
                return null; //It would be better if we could return an ITypeSymbol of string here, but return null for now

            var asSymbol = expression.ToSymbol();

            if (asSymbol is ILocalSymbol localSymbol)
            {
                return localSymbol.Type;
            }
            else if (asSymbol is IParameterSymbol parameterSymbol)
            {
                return parameterSymbol.Type; 
            }
            else if (asSymbol is IFieldSymbol fieldSymbol)
            {
                return fieldSymbol.Type;
            }
            else if (asSymbol is IPropertySymbol propertySymbol)
            {
                return propertySymbol.Type;
            }
            else if (asSymbol is ITypeSymbol typeSymbol)
            {
                return typeSymbol;
            }
            else if (asSymbol is IMethodSymbol methodSymbol)
            {
                if (methodSymbol.MethodKind.ToString() == "Constructor")
                    return methodSymbol.ReceiverType;
                else
                    return null;
            }
            else if (asSymbol is INamespaceSymbol namespaceSymbol)
            {
                return null;
            }
            else
            {
                Globals.RuntimeErrors.Add(new CannotFindUnderlyingType(expression));
                
                return null;
            }
        }

        internal static List<CallStack> GetCallStacks(this ExpressionSyntax expression)
        {
            var firstCallStack = new CallStack();
            return GetCallStacksRecursive(expression, firstCallStack);
        }

        internal static List<CallStack> GetCallStacksRecursive(this ExpressionSyntax expression, CallStack baseCallStack)
        { 
            var result = new List<CallStack>();

            if (expression is LiteralExpressionSyntax)
            {
                baseCallStack.AddLocation(expression);
                result.Add(baseCallStack);
            }
            else if (expression is IdentifierNameSyntax id)
            {
                if (baseCallStack.AddLocation(id))
                {
                    result.AddRange(GetDefinitionExpression(id, baseCallStack));
                }
                else
                {
                    result.Add(baseCallStack);
                }
            }
            else if (expression is BinaryExpressionSyntax binary)
            {
                if (baseCallStack.AddLocation(binary))
                {
                    foreach (var variable in binary.GetNonLiteralPortions())
                    {
                        var localCallStack = baseCallStack.Clone();
                        result.AddRange(GetCallStacksRecursive(variable, localCallStack));
                    }
                }
                else
                {
                    result.Add(baseCallStack);
                }
            }
            else if (expression is InterpolatedStringExpressionSyntax interpolated)
            {
                if (baseCallStack.AddLocation(interpolated))
                {
                    foreach (var variable in interpolated.GetNonLiteralPortions())
                    {
                        result.AddRange(GetDefinitionExpression(variable, baseCallStack));
                    }
                }
                else
                {
                    result.Add(baseCallStack);
                }
            }
            else if (expression is InvocationExpressionSyntax invocation)
            {
                if (baseCallStack.AddLocation(invocation))
                {
                    if (invocation.IsInvocationFromType("System.Text.StringBuilder"))
                    {
                        var model = Globals.SearchForSemanticModel(invocation.SyntaxTree);

                        if (model != null)
                        {
                            if (invocation.Expression is MemberAccessExpressionSyntax member)
                            {
                                if (member.Expression is IdentifierNameSyntax sbAsIdentifier)
                                {
                                    var root = sbAsIdentifier.Ancestors().First(a => a is MethodDeclarationSyntax containingMethod);

                                    var appendInvocations = root.DescendantNodes()
                                        .OfType<InvocationExpressionSyntax>()
                                        .Where(invocation =>
                                        {
                                            // Check if the invocation is a member access on the same StringBuilder instance
                                            var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;
                                            return memberAccess != null &&
                                                   memberAccess.Expression is IdentifierNameSyntax identifier &&
                                                   identifier.Identifier.Text == sbAsIdentifier.Identifier.Text &&
                                                   memberAccess.Name.Identifier.Text.In("Append", "AppendFormat", "AppendLine");
                                        });

                                    foreach (var appendInvocation in appendInvocations)
                                    {
                                        foreach (var arg in appendInvocation.ArgumentList.Arguments)
                                        {
                                            if (!(arg.Expression is LiteralExpressionSyntax))
                                                result.AddRange(arg.Expression.GetCallStacksRecursive(baseCallStack));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var text = invocation.ToString();

                        if (text.StartsWith("string.Format") || text.StartsWith("String.Format"))
                        {
                            for (int i = 1; i < invocation.ArgumentList.Arguments.Count; i++)
                            {
                                var arg = invocation.ArgumentList.Arguments[i].Expression;

                                if (!(arg is LiteralExpressionSyntax))
                                {
                                    result.AddRange(arg.GetCallStacksRecursive(baseCallStack));
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Add(baseCallStack);
                }
            }
            else if (expression is ConditionalExpressionSyntax)
            {
                //TODO: Figure out if we need to handle this
            }
            else if (expression is MemberAccessExpressionSyntax member)
            {
                if (baseCallStack.AddLocation(member))
                {
                    result.AddRange(GetDefinitionExpression(member, baseCallStack));
                }
                else
                { 
                    result.Add(baseCallStack); 
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return result;
        }

        //I hate this method, but it seems like the least bad solution given the limitations of finding references as symbols
        //but needing expressions to break apart string concatenations, string interpolations, etc.
        private static List<CallStack> GetDefinitionExpression(this ExpressionSyntax expression, CallStack baseCallStack)
        {
            var result = new List<CallStack>();

            var asSymbol = expression.ToSymbol();

            var localCallStack = baseCallStack.Clone();

            if (asSymbol != null && asSymbol.Locations.Any())
            {
                if (asSymbol is IParameterSymbol parameterSymbol)
                {
                    var containingMethod = parameterSymbol.ContainingSymbol as IMethodSymbol;

                    if (containingMethod != null)
                    {
                        //Skip this - CrawlTrees adds the location (might not be intuitive - refactor?)
                        //localCallStack.Locations.Add(new SourceLocation(containingMethod));

                        result.AddRange(containingMethod.CrawlTrees(localCallStack));
                    }
                }
                else if (asSymbol is ILocalSymbol localSymbol)
                {
                    if (localCallStack.AddLocation(localSymbol))
                    {
                        var syntax = localSymbol.DeclaringSyntaxReferences.Single().GetSyntax();
                        var parent = syntax.Parent as VariableDeclarationSyntax;

                        if (parent.Variables.First().Initializer != null)
                            result.AddRange(GetCallStacksRecursive(parent.Variables.First().Initializer.Value, localCallStack));
                        else
                        { 
                            result.Add(localCallStack);
                            Globals.RuntimeErrors.Add(new VariableMissingIdentifier(asSymbol));
                        }
                    }
                    else
                    {
                        result.Add(localCallStack);
                    }
                }
                else if (asSymbol is IFieldSymbol fieldSymbol)
                {
                    localCallStack.AddLocation(fieldSymbol);
                    result.Add(localCallStack);
                }
                else if (asSymbol is IPropertySymbol propertySymbol)
                {
                    //Calling method should have added this already
                    //localCallStack.Locations.Add(new SourceLocation(propertySymbol));
                    if (expression is MemberAccessExpressionSyntax member)
                    {
                        result.AddRange(GetCallStacksRecursive(member.Expression, localCallStack));
                    }
                    else
                    {
                        result.Add(localCallStack);
                    }
                }
                else if (asSymbol is IMethodSymbol methodSymbol)
                {
                    result.AddRange(methodSymbol.CrawlTrees(localCallStack));
                }
            }

            return result;
        }
    }
}
