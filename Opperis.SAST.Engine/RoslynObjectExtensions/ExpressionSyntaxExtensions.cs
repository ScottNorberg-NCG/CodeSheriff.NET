using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
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
            var model = Globals.Compilation.GetSemanticModel(expression.SyntaxTree);
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
                //It would be better to throw an exception here, but this can be triggered for one-off methods
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
                baseCallStack.AddLocation(id);

                result.AddRange(GetDefinitionExpression(id, baseCallStack));
            }
            else if (expression is BinaryExpressionSyntax binary)
            {
                baseCallStack.AddLocation(binary);

                foreach (var variable in binary.GetNonLiteralPortions())
                {
                    var localCallStack = baseCallStack.Clone();
                    result.AddRange(GetCallStacksRecursive(variable, localCallStack));
                }
            }
            else if (expression is InterpolatedStringExpressionSyntax interpolated)
            {
                baseCallStack.AddLocation(interpolated);

                foreach (var variable in interpolated.GetNonLiteralPortions())
                {
                    result.AddRange(GetDefinitionExpression(variable, baseCallStack));
                }
            }
            else if (expression is InvocationExpressionSyntax invocation)
            {
                baseCallStack.AddLocation(invocation);

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
            else if (expression is ConditionalExpressionSyntax)
            {
                //TODO: Figure out if we need to handle this
            }
            else if (expression is MemberAccessExpressionSyntax member)
            {
                baseCallStack.AddLocation(member);

                result.AddRange(GetDefinitionExpression(member, baseCallStack));
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
                        localCallStack.Locations.Add(new SourceLocation(containingMethod));

                        result.AddRange(containingMethod.CrawlTrees(localCallStack));
                    }
                }
                else if (asSymbol is ILocalSymbol localSymbol)
                {
                    localCallStack.Locations.Add(new SourceLocation(localSymbol));

                    var syntax = localSymbol.DeclaringSyntaxReferences.Single().GetSyntax();
                    var parent = syntax.Parent as VariableDeclarationSyntax;

                    result.AddRange(GetCallStacksRecursive(parent.Variables.First().Initializer.Value, localCallStack));
                }
                else if (asSymbol is IFieldSymbol fieldSymbol)
                {
                    localCallStack.Locations.Add(new SourceLocation(fieldSymbol));
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
