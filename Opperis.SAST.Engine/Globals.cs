using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Host;
using Opperis.SAST.Engine.ErrorHandling;
using Opperis.SAST.Engine.SyntaxWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine
{
    internal static class Globals
    {
        public static Solution Solution { get; set; }
        public static Compilation Compilation { get; set; }

        public static List<BaseError> RuntimeErrors { get; private set; } = new List<BaseError>();

        private static List<MethodDeclarationSyntax>? _solutionControllerMethods;

        internal static List<MethodDeclarationSyntax> SolutionControllerMethods
        {
            get
            {
                if (_solutionControllerMethods == null)
                {
                    LoadGlobalLists();
                }

                return _solutionControllerMethods;
            }
        }

        private static List<ITypeSymbol>? _entityFrameworkObjects;

        internal static List<ITypeSymbol> EntityFrameworkObjects
        {
            get
            {
                if (_entityFrameworkObjects == null)
                {
                    LoadGlobalLists();
                }

                return _entityFrameworkObjects;
            }
        }

        private static List<RazorPageBindObjectSyntaxWalker.RazorPageBindObject>? _razorPageBindObjects;

        internal static List<RazorPageBindObjectSyntaxWalker.RazorPageBindObject> RazorPageBindObjects
        {
            get
            {
                if (_razorPageBindObjects == null)
                {
                    LoadGlobalLists();
                }

                return _razorPageBindObjects;
            }
        }

        internal static void ClearErrors()
        {
            RuntimeErrors = new List<BaseError>();
        }

        internal static SemanticModel? SearchForSemanticModel(SyntaxTree tree)
        {
            foreach (var project in Solution.Projects)
            {
                var compilation = project.GetCompilationAsync().Result;

                if (compilation.ContainsSyntaxTree(tree))
                    return compilation.GetSemanticModel(tree);
            }

            return null;
        }

        internal static int MaxCallStackCount { get; set; } = 50;

        internal static int MaxCallStackDepth { get; set; } = 10;

        private static void LoadGlobalLists()
        {
            _solutionControllerMethods = new List<MethodDeclarationSyntax>();
            _entityFrameworkObjects = new List<ITypeSymbol>();
            _razorPageBindObjects = new List<RazorPageBindObjectSyntaxWalker.RazorPageBindObject>();

            foreach (var project in Globals.Solution.Projects)
            {
                Globals.Compilation = project.GetCompilationAsync().Result;

                foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
                {
                    var root = syntaxTree.GetRoot();

                    //Get Controller Methods
                    var methodFinder = new ControllerMethodSyntaxWalker();
                    methodFinder.Visit(root);

                    foreach (var method in methodFinder.Methods)
                    {
                        _solutionControllerMethods.Add(method);
                    }

                    //Get Entity Framework Objects
                    var dbTypes = new EntityFrameworkDbSetSyntaxWalker();
                    dbTypes.Visit(root);

                    foreach (var dbType in dbTypes.EntityFrameworkObjects)
                    {
                        _entityFrameworkObjects.Add(dbType);
                    }

                    //Get RazorPageBindObjects
                    var bindObjects = new RazorPageBindObjectSyntaxWalker();
                    bindObjects.Visit(root);

                    foreach (var bindObject in bindObjects.RazorPageBindObjects)
                    {
                        _razorPageBindObjects.Add(bindObject);
                    }
                }
            }
        }
    }
}
