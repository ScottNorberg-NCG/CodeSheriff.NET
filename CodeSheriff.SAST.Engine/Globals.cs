using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Host;
using CodeSheriff.SAST.Engine.ErrorHandling;
using CodeSheriff.SAST.Engine.SCA;
using CodeSheriff.SAST.Engine.SyntaxWalkers;
using CodeSheriff.SCA.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeSheriff.SAST.Engine;

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

    private static List<MethodDeclarationSyntax>? _razorPageMethods;

    internal static List<MethodDeclarationSyntax> RazorPageMethods
    {
        get
        {
            if (_razorPageMethods == null)
            {
                LoadGlobalLists();
            }

            return _razorPageMethods;
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

    private static List<AssemblyVersionInfo> _assemblies;

    internal static List<AssemblyVersionInfo> NuGetReferences
    {
        get
        {
            if (_assemblies == null)
            {
                LoadGlobalLists();
            }

            return _assemblies;
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

    internal static int MinStringLengthForSecretMatching { get; set; } = 5;

    private static void LoadGlobalLists()
    {
        _solutionControllerMethods = new List<MethodDeclarationSyntax>();
        _razorPageMethods = new List<MethodDeclarationSyntax>();
        _entityFrameworkObjects = new List<ITypeSymbol>();
        _razorPageBindObjects = new List<RazorPageBindObjectSyntaxWalker.RazorPageBindObject>();
        _assemblies = new List<AssemblyVersionInfo>();

        foreach (var project in Globals.Solution.Projects)
        {
            Globals.Compilation = project.GetCompilationAsync().Result;

            var document = XDocument.Load(project.FilePath);
            var nuGetReferences = document.Descendants().Where(n => n.Name == "PackageReference");

            foreach (var reference in nuGetReferences)
            {
                AssemblyVersionInfo info;

                var id = $"{reference.Attribute("Include").Value}|{reference.Attribute("Version").Value}";
                info = _assemblies.SingleOrDefault(a => a.UniqueIdentifier == id);

                if (info == null)
                    info = new AssemblyVersionInfo(reference.Attribute("Include").Value, reference.Attribute("Version").Value);

                info.ProjectsUsedIn.Add(project.Name);
            }

            foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();

                //Get Controller Methods
                var methodFinder = new UIProcessorMethodSyntaxWalker();
                methodFinder.Visit(root);

                foreach (var method in methodFinder.ControllerMethods)
                {
                    _solutionControllerMethods.Add(method);
                }

                foreach (var method in methodFinder.RazorPageMethods)
                {
                    _razorPageMethods.Add(method);
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
