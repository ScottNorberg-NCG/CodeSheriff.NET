using CodeSheriff.SAST.Engine;
using CodeSheriff.SAST.Engine.Findings;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.Formatting;

public static class Html
{
    public static string Generate(List<BaseFinding> findings, string fileName, bool includeErrors, Stopwatch stopwatch)
    {
        var content = new StringBuilder();

        content.AppendLine("<html>");
        content.AppendLine("<head></head>");
        content.AppendLine("<body>");

        content.AppendLine($"<h1>Findings for: {fileName}</h1>");

        foreach (var finding in findings.OrderBy(f => f.Priority.Sort))
        {
            content.AppendLine("<div style='border: 1px solid black; margin-bottom: 10px; padding-left: 10px;'>");
            GetPRow(content, "Priority", finding.Priority.Text);
            GetPRow(content, "Finding", finding.FindingText);
            GetPRow(content, "Description", finding.Description);

            if (finding.RootLocation != null)
            {
                GetPRow(content, "File", finding.RootLocation.FilePath);
                GetPRow(content, "Text", finding.RootLocation.Text);
            }

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

        if (includeErrors)
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

        return content.ToString();
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
}
