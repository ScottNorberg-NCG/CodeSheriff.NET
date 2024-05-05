using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Html.LinkTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.HtmlTagParsing
{
    internal static class CSHtmlLinkTagParser
    {
        internal static List<LinkInfo> GetLinkTags(string cshtmlContent)
        {
            var links = new List<LinkInfo>();

            string scriptTag = "<link";

            int startIndex = 0;
            int scriptStartIndex;

            // Loop through the content to find script tags
            while ((scriptStartIndex = cshtmlContent.IndexOf(scriptTag, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                // Find the end of the script tag
                int scriptEndIndex = cshtmlContent.IndexOf(">", scriptStartIndex);

                if (scriptEndIndex != -1)
                {
                    string link = cshtmlContent.Substring(scriptStartIndex, scriptEndIndex - scriptStartIndex + 1);

                    var newLink = new LinkInfo(link);

                    newLink.LineNumberStart = cshtmlContent.Substring(0, scriptStartIndex).Where(s => s == '\n').Count() + 1;

                    links.Add(newLink);

                    startIndex = scriptEndIndex + 1;
                }
                else
                {
                    // Handle cases where the script tag is not properly closed
                    break;
                }
            }

            return links;
        }

        internal static List<BaseFinding> ParseLinkTagFindings(List<LinkInfo> links, Microsoft.CodeAnalysis.TextDocument cshtmlFile)
        {
            var findings = new List<BaseFinding>();

            foreach (var link in links)
            {
                if (string.IsNullOrEmpty(link.Integrity))
                {
                    BaseFinding finding;

                    if (link.IsExternal)
                        finding = new ExternalCssMissingIntegrityHash();
                    else
                        finding = new InternalCssMissingIntegrityHash();

                    finding.RootLocation = link.GetLocation(cshtmlFile.FilePath);
                    findings.Add(finding);
                }
            }

            return findings;
        }
    }
}
