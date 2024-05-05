using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Html.LinkTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.HtmlTagParsing
{
    internal static class CSHtmlStyleTagParser
    {
        internal static List<StyleInfo> GetStyleTags(string cshtmlContent)
        {
            var styles = new List<StyleInfo>();

            string styleTag = "<style";

            int startIndex = 0;
            int scriptStartIndex;

            // Loop through the content to find script tags
            while ((scriptStartIndex = cshtmlContent.IndexOf(styleTag, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                // Find the end of the script tag
                int scriptEndIndex = cshtmlContent.IndexOf(">", scriptStartIndex);

                if (scriptEndIndex != -1)
                {
                    string style = cshtmlContent.Substring(scriptStartIndex, scriptEndIndex - scriptStartIndex + 1);

                    var newStyle = new StyleInfo(style);

                    newStyle.LineNumberStart = cshtmlContent.Substring(0, scriptStartIndex).Where(s => s == '\n').Count() + 1;

                    styles.Add(newStyle);

                    startIndex = scriptEndIndex + 1;
                }
                else
                {
                    // Handle cases where the script tag is not properly closed
                    break;
                }
            }

            return styles;
        }

        internal static List<BaseFinding> ParseStyleTagFindings(List<StyleInfo> styles, Microsoft.CodeAnalysis.TextDocument cshtmlFile)
        {
            var findings = new List<BaseFinding>();

            foreach (var style in styles)
            {
                BaseFinding finding;

                if (string.IsNullOrEmpty(style.Nonce))
                    finding = new InlineCssWithoutNonce();
                else
                    finding = new InlineCssWithNonce();

                finding.RootLocation = style.GetLocation(cshtmlFile.FilePath);
                findings.Add(finding);
            }

            return findings;
        }
    }
}
