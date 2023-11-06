using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class InvocationExpressionSyntaxExtensions
    {
        internal static bool IsInvocationOfMethod(this InvocationExpressionSyntax invocation, MethodDeclarationSyntax method)
        {
            var definition = invocation.GetDefinitionNode(method.SyntaxTree.GetRoot());

            if (definition == null)
                return false;

            if (definition is MethodDeclarationSyntax invocationDefinition)
                return definition.IsEquivalentTo(invocationDefinition);
            else
                return false;
        }
    }
}
