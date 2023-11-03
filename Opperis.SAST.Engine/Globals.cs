using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        private static List<MethodDeclarationSyntax>? _solutionControllerMethods;

        internal static List<MethodDeclarationSyntax> SolutionControllerMethods
        {
            get
            {
                if (_solutionControllerMethods == null)
                { 
                    _solutionControllerMethods = new List<MethodDeclarationSyntax>();

                    foreach (var project in Globals.Solution.Projects)
                    {
                        Globals.Compilation = project.GetCompilationAsync().Result;

                        foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
                        {
                            var root = syntaxTree.GetRoot();

                            var methodFinder = new ControllerMethodSyntaxWalker();
                            methodFinder.Visit(root);

                            foreach (var method in methodFinder.Methods)
                            {
                                _solutionControllerMethods.Add(method);
                            }
                        }
                    }
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
                    _entityFrameworkObjects = new List<ITypeSymbol>();

                    foreach (var project in Globals.Solution.Projects)
                    {
                        Globals.Compilation = project.GetCompilationAsync().Result;

                        foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
                        {
                            var root = syntaxTree.GetRoot();

                            var dbTypes = new EntityFrameworkDbSetSyntaxWalker();
                            dbTypes.Visit(root);

                            foreach (var dbType in dbTypes.EntityFrameworkObjects)
                            {
                                _entityFrameworkObjects.Add(dbType);
                            }
                        }
                    }
                }

                return _entityFrameworkObjects;
            }
        }
    }
}
