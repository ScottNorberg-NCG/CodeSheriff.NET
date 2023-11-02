using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class MethodDeclarationSyntaxExtensions
    {
        internal static bool IsUIProcessor(this MethodDeclarationSyntax method)
        {
            if (!method.Modifiers.Any(m => m.ValueText == "public"))
                return false;

            if (method.Parent is ClassDeclarationSyntax classDeclaration)
            {
                if (classDeclaration.InheritsFrom("Microsoft.AspNetCore.Mvc.RazorPages.PageModel"))
                {
                    var methodName = method.Identifier.Text;

                    if (methodName == "OnPostAsync" || methodName == "OnGetAsync")
                        return true;
                }
                else if (classDeclaration.InheritsFrom("Microsoft.AspNetCore.Mvc.Controller"))
                { 
                    //TODO: look at return type too to be sure
                    return true;                
                }
            }

            return false;
        }

        internal static List<HttpMethodInfo> GetMethodVerbs(this MethodDeclarationSyntax syntax)
        {
            var retVal = new List<HttpMethodInfo>();

            var model = Globals.Compilation.GetSemanticModel(syntax.SyntaxTree);

            if (syntax.AttributeLists.Any(al => al.Attributes.Any(a => model.GetTypeInfo(a).Type.ToString() == "Microsoft.AspNetCore.Mvc.HttpPostAttribute" ||
                                                                       model.GetTypeInfo(a).Type.ToString() == "System.Web.Mvc.HttpPostAttribute")))
                retVal.Add(new HttpMethodInfo(HttpMethodInfo.HttpMethod.Post));
            if (syntax.AttributeLists.Any(al => al.Attributes.Any(a => model.GetTypeInfo(a).Type.ToString() == "Microsoft.AspNetCore.Mvc.HttpDeleteAttribute" ||
                                                                            model.GetTypeInfo(a).Type.ToString() == "System.Web.Mvc.HttpDeleteAttribute")))
                retVal.Add(new HttpMethodInfo(HttpMethodInfo.HttpMethod.Delete));
            if (syntax.AttributeLists.Any(al => al.Attributes.Any(a => model.GetTypeInfo(a).Type.ToString() == "Microsoft.AspNetCore.Mvc.HttpGetAttribute" ||
                                                                            model.GetTypeInfo(a).Type.ToString() == "System.Web.Mvc.HttpGetAttribute")))
                retVal.Add(new HttpMethodInfo(HttpMethodInfo.HttpMethod.Get));
            if (syntax.AttributeLists.Any(al => al.Attributes.Any(a => model.GetTypeInfo(a).Type.ToString() == "Microsoft.AspNetCore.Mvc.HttpOptionsAttribute" ||
                                                                            model.GetTypeInfo(a).Type.ToString() == "System.Web.Mvc.HttpOptionsAttribute")))
                retVal.Add(new HttpMethodInfo(HttpMethodInfo.HttpMethod.Options));
            if (syntax.AttributeLists.Any(al => al.Attributes.Any(a => model.GetTypeInfo(a).Type.ToString() == "Microsoft.AspNetCore.Mvc.HttpPatchAttribute" ||
                                                                            model.GetTypeInfo(a).Type.ToString() == "System.Web.Mvc.HttpPatchAttribute")))
                retVal.Add(new HttpMethodInfo(HttpMethodInfo.HttpMethod.Patch));
            if (syntax.AttributeLists.Any(al => al.Attributes.Any(a => model.GetTypeInfo(a).Type.ToString() == "Microsoft.AspNetCore.Mvc.HttpPutAttribute" ||
                                                                            model.GetTypeInfo(a).Type.ToString() == "System.Web.Mvc.HttpPutAttribute")))
                retVal.Add(new HttpMethodInfo(HttpMethodInfo.HttpMethod.Put));
            if (syntax.AttributeLists.Any(al => al.Attributes.Any(a => model.GetTypeInfo(a).Type.ToString() == "Microsoft.AspNetCore.Mvc.HttpHeadAttribute" ||
                                                                            model.GetTypeInfo(a).Type.ToString() == "System.Web.Mvc.HttpHeadAttribute")))
                retVal.Add(new HttpMethodInfo(HttpMethodInfo.HttpMethod.Head));

            return retVal;
        }

        //TODO: Check to see if the attribute has been applied globally
        internal static bool HasCsrfProtection(this MethodDeclarationSyntax syntax)
        {
            var model = Globals.Compilation.GetSemanticModel(syntax.SyntaxTree);

            if (syntax.AttributeLists.Any(al => al.Attributes.Any(a => a.HasCsrfAttribute(model))))
                return true;

            var parentClass = syntax.Parent as ClassDeclarationSyntax;

            if (parentClass.AttributeLists.Any(al => al.Attributes.Any(a => a.HasCsrfAttribute(model))))
                return true;

            return false;
        }

        internal static bool HasName(this MethodDeclarationSyntax syntax, string className, string methodName)
        {
            if (syntax.Identifier.Text != methodName)
                return false;

            if (syntax.Parent is ClassDeclarationSyntax classSyntax)
            {
                if (classSyntax.Identifier.Text == className || classSyntax.Identifier.Text == className + "Controller")
                    return true;
            }

            return false;
        }

        internal static IEnumerable<ReturnTypeInfo> ReturnsModelType(this MethodDeclarationSyntax syntax, ITypeSymbol typeSymbol)
        {
            //var returnStatements = syntax.Body?.Statements.Where(s => s is ReturnStatementSyntax).Select(s => s as ReturnStatementSyntax).ToList();
            var returnStatements = syntax.GetReturnStatements();

            foreach (var stmt in returnStatements)
            {
                if (stmt.Expression is InvocationExpressionSyntax invocation)
                {
                    if (invocation.Expression is IdentifierNameSyntax id)
                    {
                        if (id.Identifier.Text == "View" && invocation.ArgumentList.Arguments.Count == 1)
                        {
                            var model = Globals.Compilation.GetSemanticModel(syntax.SyntaxTree);
                            var argumentType = model.GetTypeInfo(invocation.ArgumentList.Arguments[0].Expression).Type;

                            if (SymbolEqualityComparer.Default.Equals(argumentType, typeSymbol))
                                yield return new ReturnTypeInfo() { ContainingMethod = syntax, ReturnObject = invocation.ArgumentList.Arguments[0].Expression };
                        }
                    }
                }
            }
        }

        internal static List<ReturnStatementSyntax> GetReturnStatements(this MethodDeclarationSyntax syntax)
        {
            var finder = new ReturnStatementSyntaxWalker();
            finder.Visit(syntax);
            return finder.ReturnStatements;
        }

        internal class HttpMethodInfo
        {
            public enum HttpMethod
            {
                Get,
                Put,
                Delete,
                Post,
                Head,
                Trace,
                Patch,
                Connect,
                Options
            }

            public HttpMethod Method { get; set; }

            public bool SkipsCsrfChecks
            {
                get
                {
                    return Method == HttpMethod.Get || Method == HttpMethod.Head || Method == HttpMethod.Options || Method == HttpMethod.Trace;
                }
            }

            public HttpMethodInfo(HttpMethod method)
            {
                this.Method = method;
            }
        }

        internal struct ReturnTypeInfo
        {
            internal MethodDeclarationSyntax ContainingMethod { get; set; }
            internal ExpressionSyntax ReturnObject { get; set; }
        }
    }
}
