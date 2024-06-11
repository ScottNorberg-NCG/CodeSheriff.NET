using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine;
using System.Diagnostics;
using CodeSheriff.SAST.Engine.Analyzers;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using CodeSheriff.SAST.Engine.HtmlTagParsing;
using Microsoft.CodeAnalysis;

namespace CodeSheriff.CommandLine;

internal class Program
{
    private static List<BaseFinding> _findings = new List<BaseFinding>();

    static void Main(string[] args)
    {
        string? solution;
        string? outputFolder;
        bool includeTrufflehog;
        bool includeNuGet;
        bool includeHtmlOutput;
        bool includeSarifOutput;

        string[] parameters;

        if (args.Length == 0 || args[0] == "--help" || args[0] == "-h")
        {
            Console.WriteLine("Welcome to CodeSheriff.NET, the best security scanner for ASP.NET projects! To configure your project, please use these command line arguments:");
            Console.WriteLine("    Solution File (Required): /solution:[Path To Your Solution File]");
            Console.WriteLine("    Output Folder (Required): /output:[Path To Your Output Folder]");
            Console.WriteLine("    Run Trufflehog? (Optional, defaults to false): /runTrufflehog:[true/false]");
            Console.WriteLine("    Check NuGet? (Optional, defaults to true): /includeNuGetCheck:[true/false]");
            Console.WriteLine("    Output HTML? (Optional, defaults to false): /htmlOutput:[true/false]");
            Console.WriteLine("    Output SARIF? (Optional, defaults to true): /sarifOutput:[true/false]");
            Console.WriteLine();

            Console.Write("Enter command line values: ");
            parameters = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
        else
        { 
            parameters = args;
        }

        SetStringArg(parameters, "solution", out solution);
        SetStringArg(parameters, "output", out outputFolder);
        SetBooleanArg(parameters, "runTrufflehog", false, out includeTrufflehog);
        SetBooleanArg(parameters, "includeNuGetCheck", true, out includeNuGet);
        SetBooleanArg(parameters, "htmlOutput", false, out includeHtmlOutput);
        SetBooleanArg(parameters, "sarifOutput", true, out includeSarifOutput);

        if (string.IsNullOrEmpty(solution) || string.IsNullOrEmpty(outputFolder))
        {
            Console.WriteLine("ERROR: You must specify both the solution folder and output folder");
            return;
        }

        Globals.ClearErrors();
        _findings = new List<BaseFinding>();

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        //var findings = Scanner.Scan(txtSolutionFile.Text, chkNuGet.Checked, chkTrufflehog.Checked);

        PerformScan(solution, outputFolder, includeTrufflehog, includeNuGet);
        stopwatch.Stop();

        var fileName = Path.GetFileName(solution);

        if (includeHtmlOutput)
        {
            string content = CodeSheriff.Formatting.Html.Generate(_findings, fileName, false, stopwatch);

            var file = new FileInfo(solution);
            var findingsFilePath = $"{outputFolder}\\Scan {file.Name} on {DateTime.Now.ToString("yyyy-MM-dd hh-mm")}.html";

            File.WriteAllText(findingsFilePath, content);
        }

        if (includeSarifOutput)
        {
            string content = CodeSheriff.Formatting.Sarif.Generate(_findings);

            var file = new FileInfo(solution);
            var findingsFilePath = $"{outputFolder}\\Scan {file.Name} on {DateTime.Now.ToString("yyyy-MM-dd hh-mm")}.sarif";

            File.WriteAllText(findingsFilePath, content);
        }
    }

    static void SetStringArg(string[] inputValues, string name, out string? destination)
    {
        var input = inputValues.SingleOrDefault(a => a.StartsWith($"/{name}:"));
        if (input == null)
        {
            destination = null;
            return;
        }

        var value = input.Replace("\"", "").Replace("'", "").Substring(input.IndexOf(":") + 1);

        destination = value;
    }

    static void SetBooleanArg(string[] inputValues, string name, bool defaultValue, out bool destination)
    {
        var input = inputValues.SingleOrDefault(a => a.StartsWith($"/{name}:"));
        if (input == null)
        {
            destination = defaultValue;
            return;
        }

        var inputArray = input.Split(":");

        if (inputArray.Length != 2)
        {
            destination = defaultValue;
            return;
        }

        if (!bool.TryParse(inputArray[1], out destination))
            destination = defaultValue;
    }

    private static void PerformScan(string solution, string folder, bool includeTrufflehog, bool runNuGetCheck)
    {
        if (!MSBuildLocator.IsRegistered)
            MSBuildLocator.RegisterDefaults();

        _findings = new List<BaseFinding>();

        using (var workspace = MSBuildWorkspace.Create())
        {
            Globals.Solution = workspace.OpenSolutionAsync(solution).Result;

            if (includeTrufflehog)
            {
                foreach (var project in Globals.Solution.Projects)
                {
                    foreach (var doc in project.AdditionalDocuments)
                    {
                        _findings.AddRange(TrufflehogAnalyzer.RunTrufflehogScan(doc.FilePath));
                    }

                    foreach (var doc in project.Documents)
                    {
                        _findings.AddRange(TrufflehogAnalyzer.RunTrufflehogScan(doc.FilePath));
                    }
                }
            }

            if (runNuGetCheck)
            {
                _findings.AddRange(ScaAnalyzer.GetVulnerableNuGetPackages());
            }

            _findings.AddRange(OverpostingViaControllerAnalyzer.FindEFObjectsAsParameters());
            _findings.AddRange(CsrfAnalyzer.FindCsrfIssues());
            _findings.AddRange(ValueShadowingAnalyzer.FindValueShadowingPossibilities());

            var projectIndex = 0;
            var projectCount = Globals.Solution.Projects.Count();

            foreach (var project in Globals.Solution.Projects)
            {
                Globals.Compilation = project.GetCompilationAsync().Result;

                foreach (var cshtmlFile in project.AdditionalDocuments.Where(d => d.FilePath.EndsWith(".cshtml")))
                {
                    ParseJavaScriptTags(cshtmlFile);
                    ParseLinkTags(cshtmlFile);
                    ParseStyleTags(cshtmlFile);
                }

                var syntaxTreeIndex = 0;
                var syntaxTreeCount = Globals.Compilation.SyntaxTrees.Count();

                foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
                {
                    var root = syntaxTree.GetRoot();

                    _findings.AddRange(SQLInjectionAnalyzer.GetSQLInjections(root));
                    _findings.AddRange(DatabaseConnectionOpenAnalyzer.FindUnsafeDatabaseConnectionOpens(root));
                    _findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.GetMisconfiguredProperties(root));
                    _findings.AddRange(SymmetricAlgorithmAnalyzer.FindDeprecatedAlgorithms(root));
                    _findings.AddRange(HashAlgorithmAnalyzer.FindDeprecatedAlgorithms(root));
                    _findings.AddRange(ExternalRedirectAnalyzer.FindProblematicExternalRedirects(root));
                    _findings.AddRange(HardCodedConnectionStringAnalyzer.FindHardCodedConnectionStrings(root));

                    var rawAnalyzer = new HtmlRawAnalyzer();
                    _findings.AddRange(rawAnalyzer.FindXssIssues(root));

                    var helperAnalyzer = new HtmlHelperAnalyzer();
                    _findings.AddRange(helperAnalyzer.FindXssIssues(root));

                    _findings.AddRange(OverpostingViaBindObjectAnalyzer.FindEFObjectsAsBindObjects(root));

                    SearchForCookieManipulations(root);
                    SearchForFileManipulations(root);

                    _findings.AddRange(ModelValidationAnalyzer.FindMissingModelValidations(root));
                    _findings.AddRange(JwtTokenMisconfigurationAnalyzer.FindMisconfigurations(root));
                    _findings.AddRange(RSAKeySizeInConstructorAnalyzer.FindInadequateKeyLengths(root));
                    _findings.AddRange(RSAKeySizeInPropertyAnalyzer.FindInadequateKeyLengths(root));
                    _findings.AddRange(SQLInjectionViaEntityFrameworkAnalyzer.GetSQLInjections(root));
                    _findings.AddRange(PasswordSignInAnalyzer.FindDisabledLockouts(root));
                    _findings.AddRange(IUserStoreAnalyzer.FindMisconfiguredUserStores(root));

                    syntaxTreeIndex++;
                    //if (includeSecrets)
                    //    SearchForSecrets(findings, root, rules);
                }

                projectIndex++;
            }
        }
    }

    private static void ParseJavaScriptTags(TextDocument? cshtmlFile)
    {
        var scripts = CSHtmlScriptTagParser.GetScriptTags(cshtmlFile.GetTextAsync().Result.ToString());
        _findings.AddRange(CSHtmlScriptTagParser.ParseJavaScriptFindings(scripts, cshtmlFile));
    }

    private static void ParseLinkTags(TextDocument? cshtmlFile)
    {
        var links = CSHtmlLinkTagParser.GetLinkTags(cshtmlFile.GetTextAsync().Result.ToString());
        _findings.AddRange(CSHtmlLinkTagParser.ParseLinkTagFindings(links, cshtmlFile));
    }

    private static void ParseStyleTags(TextDocument? cshtmlFile)
    {
        var styleTags = CSHtmlStyleTagParser.GetStyleTags(cshtmlFile.GetTextAsync().Result.ToString());
        _findings.AddRange(CSHtmlStyleTagParser.ParseStyleTagFindings(styleTags, cshtmlFile));
    }

    private static void SearchForCookieManipulations(SyntaxNode root)
    {
        _findings.AddRange(CookieConfigurationAnalyzer.FindMisconfiguredCookies(root));
    }

    private static void SearchForFileManipulations(SyntaxNode root)
    {
        _findings.AddRange(FileManipulationAnalyzer.FindFileManipulations(root));
        _findings.AddRange(FileResultAnalyzer.GetFileResults(root));
    }
}
