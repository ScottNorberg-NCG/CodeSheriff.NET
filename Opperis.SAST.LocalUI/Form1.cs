using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Opperis.SAST.Engine;
using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.HtmlTagParsing;
using Opperis.SAST.Engine.SyntaxWalkers;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Opperis.SAST.LocalUI;

public partial class Form1 : Form
{
    private static readonly string COMPLETED = "100%";
    private static List<BaseFinding> _findings = new List<BaseFinding>();

    public Form1()
    {
        InitializeComponent();

#if DEBUG
        txtResultsFolder.Text = "C:\\temp\\ScanResults";
        //txtSolutionFile.Text = "C:\\Users\\scott\\Downloads\\sentry-dotnet-main\\sentry-dotnet-main\\Sentry.NoMobile.sln";
        txtSolutionFile.Text = "C:\\Users\\scott\\Source\\repos\\VulnerabilityBuffet2\\AspNetCore\\NCG.SecurityDetection.VulnerabilityBuffet.sln";
        chkIncludeBindings.Checked = true;
        chkTrufflehog.Checked = true;
#endif
    }

    private void btnChooseSolution_Click(object sender, EventArgs e)
    {
        var result = openFileDialog1.ShowDialog();
        if (result == DialogResult.OK)
        {
            txtSolutionFile.Text = openFileDialog1.FileName;
        }
    }

    private void btnChooseFolder_Click(object sender, EventArgs e)
    {
        var result = folderBrowserDialog1.ShowDialog();

        if (result == DialogResult.OK)
        {
            txtResultsFolder.Text = folderBrowserDialog1.SelectedPath;
        }
    }

    private void btnScan_Click(object sender, EventArgs e)
    {
        Globals.ClearErrors();
        _findings = new List<BaseFinding>();

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        //var findings = Scanner.Scan(txtSolutionFile.Text, chkNuGet.Checked, chkTrufflehog.Checked);

        RunScan();
        stopwatch.Stop();

        var content = new StringBuilder();

        content.AppendLine("<html>");
        content.AppendLine("<head></head>");
        content.AppendLine("<body>");

        var fileName = Path.GetFileName(txtSolutionFile.Text);

        content.AppendLine($"<h1>Findings for: {fileName}</h1>");

        foreach (var finding in _findings.OrderBy(f => f.Priority.Sort))
        {
            content.AppendLine("<div style='border: 1px solid black; margin-bottom: 10px; padding-left: 10px;'>");
            GetPRow(content, "Priority", finding.Priority.Text);
            GetPRow(content, "Finding", finding.FindingText);
            GetPRow(content, "Description", finding.Description);
            GetPRow(content, "File", finding.RootLocation.FilePath);
            GetPRow(content, "Text", finding.RootLocation.Text);

            content.AppendLine("<p>");
            content.AppendLine("<div style='font-weight: bold;'>Call Stacks</div><div>");

            foreach (var cs in finding.CallStacks)
            {
                foreach (var location in cs.Locations)
                {
                    content.Append(System.Web.HttpUtility.HtmlEncode(location.ToString()));
                    content.AppendLine("<br />");
                }

                content.AppendLine("<hr />");
            }

            content.AppendLine("</div></p>");
            content.AppendLine("</div>");
        }

        if (chkIncludeBindings.Checked)
        {
            content.AppendLine("<h2>Diagnostic info</h2>");
            foreach (var error in Globals.RuntimeErrors)
            {
                string message;

                if (error.CodeLocation != null)
                    message = error.CodeLocation.ToString();
                else
                    message = error.BaseException.Message;

                var stackTrace = error.BaseException != null ? error.BaseException.ToString() : "N/A";

                content.AppendLine("<div>");
                content.AppendLine($"<div>Type: {error.Category.ToString()}</div>");
                content.AppendLine($"<div>Source Code Location: {System.Web.HttpUtility.HtmlEncode(message)}</div>");
                content.AppendLine($"<div><pre>Stack trace: {System.Web.HttpUtility.HtmlEncode(stackTrace)}</pre></div>");
                content.AppendLine("<hr />");
                content.AppendLine("</div>");
            }
        }

        content.AppendLine($"<p>Scan completed in {stopwatch.Elapsed.Minutes} minutes and {stopwatch.Elapsed.Seconds} seconds</p>");
        content.AppendLine("</body>");
        content.AppendLine("</html>");

        var findingsFilePath = $"{txtResultsFolder.Text}\\Scan Results {DateTime.Now.ToString("MM-dd-yyyy hh-mm")}.html";

        File.WriteAllText(findingsFilePath, content.ToString());

        lblStatusStep.UpdateText(COMPLETED);
        MessageBox.Show("Completed");
    }

    private static void GetPRow(StringBuilder sb, string label, string content)
    {
        GetPRow(sb, label, new string[] { content });
    }

    private static void GetPRow(StringBuilder sb, string label, string[] content)
    {
        sb.AppendLine("<p>");
        sb.AppendLine("<div style='font-weight: bold;'>");
        sb.AppendLine(System.Web.HttpUtility.HtmlEncode(label));
        sb.AppendLine("</div>");
        sb.AppendLine("<div>");
        sb.AppendLine(string.Join("<hr>", content.Select(c => System.Web.HttpUtility.HtmlEncode(c))));
        sb.AppendLine("</div>");
        sb.AppendLine("</p>");
    }

    private void lblTrufflehog_Click(object sender, EventArgs e)
    {
        MessageBox.Show("To run Trufflehog, you must have it installed on the machine running the scan. For more information about Trufflehog, please see https://github.com/trufflesecurity/trufflehog.");
    }

    private void RunScan()
    {
        lblStatusStep.UpdateText("Loading projects");

        if (!MSBuildLocator.IsRegistered)
            MSBuildLocator.RegisterDefaults();

        _findings = new List<BaseFinding>();

        if (!chkTrufflehog.Checked)
            lblStatusTrufflehog.UpdateText("N/A");

        if (!chkNuGet.Checked)
            lblStatusNuGet.UpdateText("N/A");

        using (var workspace = MSBuildWorkspace.Create())
        {
            Globals.Solution = workspace.OpenSolutionAsync(txtSolutionFile.Text).Result;

            _findings.AddRange(OverpostingAnalyzer.FindEFObjectsAsParameters());

            if (chkTrufflehog.Checked)
            {
                lblStatusStep.UpdateText("Running Trufflehog scans");

                var totalDocs = Globals.Solution.Projects.SelectMany(p => p.AdditionalDocuments).Count() + Globals.Solution.Projects.SelectMany(p => p.Documents).Count();
                var completed = 0;

                foreach (var project in Globals.Solution.Projects)
                {
                    foreach (var doc in project.AdditionalDocuments)
                    {
                        _findings.AddRange(TrufflehogAnalyzer.RunTrufflehogScan(doc.FilePath));
                        completed++;
                        lblStatusTrufflehog.UpdatePercentComplete(completed, totalDocs);
                        RefreshFindingCount();
                    }

                    foreach (var doc in project.Documents)
                    {
                        _findings.AddRange(TrufflehogAnalyzer.RunTrufflehogScan(doc.FilePath));
                        completed++;
                        lblStatusTrufflehog.UpdatePercentComplete(completed, totalDocs);
                        RefreshFindingCount();
                    }
                }
            }

            lblStatusTrufflehog.UpdateText("100%");

            if (chkNuGet.Checked)
            {
                lblStatusStep.UpdateText("Looking for vulnerable components (via NuGet)");

                _findings.AddRange(ScaAnalyzer.GetVulnerableNuGetPackages());
                lblStatusNuGet.UpdateText(COMPLETED);
                RefreshFindingCount();
            }

            var projectIndex = 1;
            var projectCount = Globals.Solution.Projects.Count();

            foreach (var project in Globals.Solution.Projects)
            {
                lblStatusStep.Text = $"Scanning project {projectIndex} of {projectCount} ({project.Name})";

                Globals.Compilation = project.GetCompilationAsync().Result;

                foreach (var cshtmlFile in project.AdditionalDocuments.Where(d => d.FilePath.EndsWith(".cshtml")))
                {
                    ParseJavaScriptTags(cshtmlFile);
                    ParseLinkTags(cshtmlFile);
                    ParseStyleTags(cshtmlFile);
                }

                lblStatusJSTag.UpdatePercentComplete(projectIndex, Globals.Solution.Projects.Count());
                lblStatusLinkTag.UpdatePercentComplete(projectIndex, Globals.Solution.Projects.Count());
                lblStatusStyleTag.UpdatePercentComplete(projectIndex, Globals.Solution.Projects.Count());

                RefreshFindingCount();

                var syntaxTreeIndex = 1;
                var syntaxTreeCount = Globals.Compilation.SyntaxTrees.Count();

                foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
                {
                    var root = syntaxTree.GetRoot();

                    var databaseCalls = new DatabaseCommandTextSyntaxWalker();
                    _findings.AddRange(SQLInjectionAnalyzer.GetSQLInjections(databaseCalls, root));
                    lblStatusSqlInjection.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    //TODO: Refactor this so we go off the global list to avoid doing the analysis twice
                    var controllerMethods = new UIProcessorMethodSyntaxWalker();
                    _findings.AddRange(CsrfAnalyzer.FindCsrfIssues(controllerMethods, root));
                    _findings.AddRange(ValueShadowingAnalyzer.FindValueShadowingPossibilities(controllerMethods, root));
                    lblStatusCsrf.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    lblStatusValueShadowing.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var databaseConnections = new DatabaseConnectionOpenSyntaxWalker();
                    _findings.AddRange(DatabaseConnectionOpenAnalyzer.FindUnsafeDatabaseConnectionOpens(databaseConnections, root));
                    lblStatusUnclosedConnection.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var cryptoKeyFinder = new SymmetricCryptographyPropertySyntaxWalker();
                    _findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindHardCodedKeys(cryptoKeyFinder, root));
                    _findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindHardCodedIVs(cryptoKeyFinder, root));
                    _findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.FindECBUses(cryptoKeyFinder, root));
                    lblStatusCryptoKeys.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    lblStatusCryptoIVs.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    lblStatusCryptoECB.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var cryptoAlgorithmFinder = new SymmetricAlgorithmSyntaxWalker();
                    _findings.AddRange(SymmetricAlgorithmAnalyzer.FindDeprecatedAlgorithms(cryptoAlgorithmFinder, root));
                    lblStatusDeprecatedCrypto.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var hashAlgorithmFinder = new ComputeHashSyntaxWalker();
                    _findings.AddRange(HashAlgorithmAnalyzer.FindDeprecatedAlgorithms(hashAlgorithmFinder, root));
                    lblStatusHashingAlgorithm.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var externalRedirects = new ExternalRedirectSyntaxWalker();
                    _findings.AddRange(ExternalRedirectAnalyzer.FindProblematicExternalRedirects(externalRedirects, root));
                    lblStatusRedirect.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var hardCodedConnectionStrings = new DatabaseConnectionStringSyntaxWalker();
                    _findings.AddRange(HardCodedConnectionStringAnalyzer.FindHardCodedConnectionStrings(hardCodedConnectionStrings, root));
                    lblStatusConnectionString.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var problematicHtmlRaws = new HtmlRawSyntaxWalker();
                    var rawAnalyzer = new HtmlRawAnalyzer();
                    _findings.AddRange(rawAnalyzer.FindXssIssues(problematicHtmlRaws, root));
                    lblStatusXssViaRaw.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var problematicHtmlHelpers = new HtmlHelperSyntaxWalker();
                    var helperAnalyzer = new HtmlHelperAnalyzer();
                    _findings.AddRange(helperAnalyzer.FindXssIssues(problematicHtmlHelpers, root));
                    lblStatusXssViaHelper.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var overpostingsAsBindObjects = new RazorPageBindObjectSyntaxWalker();
                    _findings.AddRange(OverpostingAnalyzer.FindEFObjectsAsBindObjects(overpostingsAsBindObjects, root));
                    lblStatusOverposting.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    SearchForCookieManipulations(root);
                    lblStatusCookies.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    SearchForFileManipulations(root);
                    lblStatusFile.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var uiMethodWalker = new UIProcessorMethodSyntaxWalker();
                    _findings.AddRange(ModelValidationAnalyzer.FindMissingModelValidations(uiMethodWalker, root));
                    lblStatusOverposting.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var jwtParameterWalker = new JwtTokenParameterSetSyntaxWalker();
                    _findings.AddRange(JwtTokenMisconfigurationAnalyzer.FindMisconfigurations(jwtParameterWalker, root));
                    lblStatusJWT.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var rsaConstructorWalker = new RSAConstructorSyntaxWalker();
                    _findings.AddRange(RSAKeySizeInConstructorAnalyzer.FindInadequateKeyLengths(rsaConstructorWalker, root));
                    var rsaKeySizeWalker = new RSAKeySizeSyntaxWalker();
                    _findings.AddRange(RSAKeySizeInPropertyAnalyzer.FindInadequateKeyLengths(rsaKeySizeWalker, root));
                    lblStatusRSA.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var dbCallViaEFWalker = new EntityFrameworkDbCallSyntaxWalker();
                    _findings.AddRange(SQLInjectionViaEntityFrameworkAnalyzer.GetSQLInjections(dbCallViaEFWalker, root));
                    lblStatusSQLiViaEF.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var passwordLockoutWalker = new PasswordSignInSyntaxWalker();
                    _findings.AddRange(PasswordSignInAnalyzer.FindDisabledLockouts(passwordLockoutWalker, root));
                    lblStatusPasswordLockout.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var iUserStoreWalker = new IUserStoreSyntaxWalker();
                    _findings.AddRange(IUserStoreAnalyzer.FindMisconfiguredUserStores(iUserStoreWalker, root));
                    lblStatusIUserStore.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    syntaxTreeIndex++;
                    //if (includeSecrets)
                    //    SearchForSecrets(findings, root, rules);
                }

                projectIndex++;
            }

            lblStatusStep.UpdateText("Scan complete, writing to HTML file");
        }
    }

    private void ParseJavaScriptTags(TextDocument? cshtmlFile)
    {
        var scripts = CSHtmlScriptTagParser.GetScriptTags(cshtmlFile.GetTextAsync().Result.ToString());
        _findings.AddRange(CSHtmlScriptTagParser.ParseJavaScriptFindings(scripts, cshtmlFile));
    }

    private void ParseLinkTags(TextDocument? cshtmlFile)
    {
        var links = CSHtmlLinkTagParser.GetLinkTags(cshtmlFile.GetTextAsync().Result.ToString());
        _findings.AddRange(CSHtmlLinkTagParser.ParseLinkTagFindings(links, cshtmlFile));
    }

    private void ParseStyleTags(TextDocument? cshtmlFile)
    {
        var styleTags = CSHtmlStyleTagParser.GetStyleTags(cshtmlFile.GetTextAsync().Result.ToString());
        _findings.AddRange(CSHtmlStyleTagParser.ParseStyleTagFindings(styleTags, cshtmlFile));
    }

    private void SearchForCookieManipulations(SyntaxNode root)
    {
        var cookieConfigurationWalker = new CookieAppendSyntaxWalker();
        _findings.AddRange(CookieConfigurationAnalyzer.FindMisconfiguredCookies(cookieConfigurationWalker, root));
    }

    private void SearchForFileManipulations(SyntaxNode root)
    {
        var fileManipulationWalker = new FileManipulationSyntaxWalker();
        _findings.AddRange(FileManipulationAnalyzer.FindFileManipulations(fileManipulationWalker, root));

        var fileResultWalker = new FileResultSyntaxWalker();
        _findings.AddRange(FileResultAnalyzer.GetFileResults(fileResultWalker, root));
    }

    private void RefreshFindingCount()
    {
        lblStatusFindings.Text = $"Findings: {_findings.Count}";
        lblStatusFindings.Refresh();
    }
}