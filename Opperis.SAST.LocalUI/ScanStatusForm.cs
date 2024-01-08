using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Opperis.SAST.Engine;
using Opperis.SAST.Engine.Analyzers;
using Opperis.SAST.Engine.DataAccessAnalysis;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.HtmlTagParsing;
using Opperis.SAST.Engine.SyntaxWalkers;
using Opperis.SAST.LocalUI.ExtensionMethods;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Opperis.SAST.LocalUI;

public partial class ScanStatusForm : Form
{
    private static readonly string COMPLETED = "100%";
    private static List<BaseFinding> _findings = new List<BaseFinding>();

    public ScanStatusForm()
    {
        InitializeComponent();
    }

    public void RunScan(string solution, string folder, bool includeBindings, bool includeTrufflehog, bool includeNuGet)
    {
        this.Show();
        this.Refresh();

        Globals.ClearErrors();
        _findings = new List<BaseFinding>();

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        //var findings = Scanner.Scan(txtSolutionFile.Text, chkNuGet.Checked, chkTrufflehog.Checked);

        PerformScan(solution, folder, includeTrufflehog, includeNuGet);
        stopwatch.Stop();

        var content = new StringBuilder();

        content.AppendLine("<html>");
        content.AppendLine("<head></head>");
        content.AppendLine("<body>");

        var fileName = Path.GetFileName(solution);

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

        if (includeBindings)
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

        var file = new FileInfo(solution);
        var findingsFilePath = $"{folder}\\Findings for {file.Name} on {DateTime.Now.ToString("yyyy-MM-dd hh-mm")}.html";

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

    private void PerformScan(string solution, string folder, bool includeTrufflehog, bool runNuGetCheck)
    {
        lblStatusStep.UpdateText("Loading projects");

        if (!MSBuildLocator.IsRegistered)
            MSBuildLocator.RegisterDefaults();

        _findings = new List<BaseFinding>();

        if (!includeTrufflehog)
            lblStatusTrufflehog.UpdateText("N/A");

        if (!runNuGetCheck)
            lblStatusNuGet.UpdateText("N/A");

        using (var workspace = MSBuildWorkspace.Create())
        {
            Globals.Solution = workspace.OpenSolutionAsync(solution).Result;

            lblStatusStep.UpdateText("Running Database Access Analysis");
            GenerateDatabaseAccessReport(solution, folder);
            lblStatusDBAnalysis.UpdateText("100%");

            if (includeTrufflehog)
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

            if (runNuGetCheck)
            {
                lblStatusStep.UpdateText("Looking for vulnerable components (via NuGet)");

                _findings.AddRange(ScaAnalyzer.GetVulnerableNuGetPackages());
                lblStatusNuGet.UpdateText(COMPLETED);
                RefreshFindingCount();
            }

            _findings.AddRange(OverpostingViaControllerAnalyzer.FindEFObjectsAsParameters());
            _findings.AddRange(CsrfAnalyzer.FindCsrfIssues());
            _findings.AddRange(ValueShadowingAnalyzer.FindValueShadowingPossibilities());

            lblStatusOverposting.UpdateText("100%");
            lblStatusCsrf.UpdateText("100%");
            lblStatusValueShadowing.UpdateText("100%");
            RefreshFindingCount();

            var projectIndex = 0;
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

                var syntaxTreeIndex = 0;
                var syntaxTreeCount = Globals.Compilation.SyntaxTrees.Count();

                foreach (var syntaxTree in Globals.Compilation.SyntaxTrees)
                {
                    var root = syntaxTree.GetRoot();

                    _findings.AddRange(SQLInjectionAnalyzer.GetSQLInjections(root));
                    lblStatusSqlInjection.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(DatabaseConnectionOpenAnalyzer.FindUnsafeDatabaseConnectionOpens(root));
                    lblStatusUnclosedConnection.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(SymmetricAlgorithmPropertyAnalyzer.GetMisconfiguredProperties(root));
                    lblStatusCryptoKeys.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    lblStatusCryptoIVs.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    lblStatusCryptoECB.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(SymmetricAlgorithmAnalyzer.FindDeprecatedAlgorithms(root));
                    lblStatusDeprecatedCrypto.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(HashAlgorithmAnalyzer.FindDeprecatedAlgorithms(root));
                    lblStatusHashingAlgorithm.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(ExternalRedirectAnalyzer.FindProblematicExternalRedirects(root));
                    lblStatusRedirect.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(HardCodedConnectionStringAnalyzer.FindHardCodedConnectionStrings(root));
                    lblStatusConnectionString.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var rawAnalyzer = new HtmlRawAnalyzer();
                    _findings.AddRange(rawAnalyzer.FindXssIssues(root));
                    lblStatusXssViaRaw.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    var helperAnalyzer = new HtmlHelperAnalyzer();
                    _findings.AddRange(helperAnalyzer.FindXssIssues(root));
                    lblStatusXssViaHelper.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(OverpostingViaBindObjectAnalyzer.FindEFObjectsAsBindObjects(root));
                    lblStatusOverposting.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    SearchForCookieManipulations(root);
                    lblStatusCookies.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    SearchForFileManipulations(root);
                    lblStatusFile.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(ModelValidationAnalyzer.FindMissingModelValidations(root));
                    lblStatusInputValidation.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(JwtTokenMisconfigurationAnalyzer.FindMisconfigurations(root));
                    lblStatusJWT.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(RSAKeySizeInConstructorAnalyzer.FindInadequateKeyLengths(root));
                    _findings.AddRange(RSAKeySizeInPropertyAnalyzer.FindInadequateKeyLengths(root));
                    lblStatusRSA.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(SQLInjectionViaEntityFrameworkAnalyzer.GetSQLInjections(root));
                    lblStatusSQLiViaEF.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(PasswordSignInAnalyzer.FindDisabledLockouts(root));
                    lblStatusPasswordLockout.UpdatePercentComplete(projectIndex, projectCount, syntaxTreeIndex, syntaxTreeCount);
                    RefreshFindingCount();

                    _findings.AddRange(IUserStoreAnalyzer.FindMisconfiguredUserStores(root));
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

    private static void GenerateDatabaseAccessReport(string solution, string folder)
    {
        var dataPoints = DataAccessAnalyzer.FindDataAccessPoints();

        var roles = dataPoints.Where(dp => dp.Roles != null).SelectMany(dp => dp.Roles).Distinct().OrderBy(s => s).ToList();

        var content = new StringBuilder();

        content.AppendLine("<html>");
        content.AppendLine("<head></head>");
        content.AppendLine("<body>");

        var fileName = Path.GetFileName(solution);

        content.AppendLine($"<h1>EF Analysis for: {fileName}</h1>");
        content.AppendLine("<h2>Writes via EF</h2>");
        content.AppendLine("<table width=\"100%\">");
        content.AppendLine("<tr>");
        content.Append("<th>Object</th>");
        content.Append("<th>Property</th>");
        content.Append("<th># of Writes</th>");
        content.Append("<th>% Unauthenticated</th>");
        content.Append("<th>% Auth (No Role)</th>");

        foreach (var role in roles)
        { 
            content.Append($"<th>% Auth, ({role})</th>");
        }
        
        content.AppendLine("</tr>");

        var writes = dataPoints.Where(dp => dp.DataDirection == DataAccessItem.Direction.ToDatabase).ToList();

        var writeProperties = writes.Select(dp => new { dp.ContainingType, dp.PropertyName }).Distinct().ToList();

        foreach (var access in writeProperties.OrderBy(dp => dp.ContainingType).ThenBy(dp => dp.PropertyName))
        {
            var localWrites = writes.Where(dp => dp.ContainingType == access.ContainingType && dp.PropertyName == access.PropertyName).ToList();

            content.AppendLine("<tr>");

            content.Append($"<td>{access.ContainingType}</td>");
            content.Append($"<td>{access.PropertyName}</td>");
            content.Append($"<td>{localWrites.Count}</td>");
            content.Append($"<td>{localWrites.Count(w => !w.IsAuthorizedAccess).AsPercentage(localWrites.Count)}</td>");
            content.Append($"<td>{localWrites.Count(w => w.IsAuthorizedAccess && w.Roles.Count == 0).AsPercentage(localWrites.Count)}</td>");

            foreach (var role in roles)
            {
                content.Append($"<td>{localWrites.Count(w => w.Roles != null && w.Roles.Contains(role)).AsPercentage(localWrites.Count)}</td>");
            }

            content.AppendLine("</tr>");
        }

        content.AppendLine("</table>");
        content.AppendLine("</body>");
        content.AppendLine("</html>");

        var file = new FileInfo(solution);
        var findingsFilePath = $"{folder}\\EF Analysis for {file.Name} on {DateTime.Now.ToString("yyyy-MM-dd hh-mm")}.html";

        File.WriteAllText(findingsFilePath, content.ToString());
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
        _findings.AddRange(CookieConfigurationAnalyzer.FindMisconfiguredCookies(root));
    }

    private void SearchForFileManipulations(SyntaxNode root)
    {
        _findings.AddRange(FileManipulationAnalyzer.FindFileManipulations(root));
        _findings.AddRange(FileResultAnalyzer.GetFileResults(root));
    }

    private void RefreshFindingCount()
    {
        lblStatusFindings.Text = $"Findings: {_findings.Count}";
        lblStatusFindings.Refresh();
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }
}