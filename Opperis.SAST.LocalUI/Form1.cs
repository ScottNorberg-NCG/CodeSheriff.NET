using Opperis.SAST.Engine;
using System.Diagnostics;
using System.Text;

namespace Opperis.SAST.LocalUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var findings = Scanner.Scan(txtSolutionFile.Text);
            stopwatch.Stop();

            var content = new StringBuilder();

            content.AppendLine("<html>");
            content.AppendLine("<head></head>");
            content.AppendLine("<body>");

            var fileName = Path.GetFileName(txtSolutionFile.Text);

            content.AppendLine($"<h1>Findings for: {fileName}</h1>");

            foreach (var finding in findings.OrderBy(f => f.Priority.Sort))
            {
                content.AppendLine("<div style='border: 1px solid black; margin-bottom: 10px; padding-left: 10px;'>");
                GetPRow(content, "Priority", finding.Priority.Text);
                GetPRow(content, "Finding", finding.FindingText);
                GetPRow(content, "Description", finding.Description);
                GetPRow(content, "File", finding.CallStacks.First().Locations.First().FilePath);
                GetPRow(content, "Text", finding.CallStacks.First().Locations.First().Text);

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

            content.AppendLine("</table>");
            content.AppendLine($"<p>Scan completed in {stopwatch.Elapsed.Minutes} minutes and {stopwatch.Elapsed.Seconds} seconds</p>");
            content.AppendLine("</body>");
            content.AppendLine("</html>");

            var findingsFilePath = $"{txtResultsFolder.Text}\\Scan Results {DateTime.Now.ToString("MM-dd-yyyy hh-mm")}.html";

            File.WriteAllText(findingsFilePath, content.ToString());

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
            sb.AppendLine(label);
            sb.AppendLine("</div>");
            sb.AppendLine("<div>");
            sb.AppendLine(string.Join("<hr>", content.Select(c => System.Web.HttpUtility.HtmlEncode(c))));
            sb.AppendLine("</div>");
            sb.AppendLine("</p>");
        }
    }
}