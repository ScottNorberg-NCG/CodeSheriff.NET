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

public partial class ConfigurationForm : Form
{
    public ConfigurationForm()
    {
        InitializeComponent();

#if DEBUG
        txtResultsFolder.Text = "C:\\temp\\ScanResults";
        //txtSolutionFile.Text = "C:\\Users\\scott\\Downloads\\sentry-dotnet-main\\sentry-dotnet-main\\Sentry.NoMobile.sln";
        //txtSolutionFile.Text = "C:\\Users\\scott\\Source\\repos\\VulnerabilityBuffet2\\AspNetCore\\NCG.SecurityDetection.VulnerabilityBuffet.sln";
        txtSolutionFile.Text = "C:\\Users\\scott\\Source\\repos\\SASTTest\\SASTTest.sln";
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
        var scanForm = new ScanStatusForm();
        scanForm.RunScan(txtSolutionFile.Text, txtResultsFolder.Text, chkIncludeBindings.Checked, chkTrufflehog.Checked, chkNuGet.Checked);
    }

    private void lblTrufflehog_Click(object sender, EventArgs e)
    {
        MessageBox.Show("To run Trufflehog, you must have it installed on the machine running the scan. For more information about Trufflehog, please see https://github.com/trufflesecurity/trufflehog.");
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }
}