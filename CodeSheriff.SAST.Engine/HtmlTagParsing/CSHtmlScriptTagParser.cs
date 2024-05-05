using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.Findings.Html.JavaScriptTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.HtmlTagParsing
{
    internal static class CSHtmlScriptTagParser
    {
        internal static List<ScriptInfo> GetScriptTags(string cshtmlContent)
        {
            var scripts = new List<ScriptInfo>();

            string scriptTag = "<script";

            int startIndex = 0;
            int scriptStartIndex;

            // Loop through the content to find script tags
            while ((scriptStartIndex = cshtmlContent.IndexOf(scriptTag, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                // Find the end of the script tag
                int scriptEndIndex = cshtmlContent.IndexOf(">", scriptStartIndex);

                if (scriptEndIndex != -1)
                {
                    string script = cshtmlContent.Substring(scriptStartIndex, scriptEndIndex - scriptStartIndex + 1);

                    var newScript = new ScriptInfo(script);

                    newScript.LineNumberStart = cshtmlContent.Substring(0, scriptStartIndex).Where(s => s == '\n').Count() + 1;

                    scripts.Add(newScript);

                    startIndex = scriptEndIndex + 1;
                }
                else
                {
                    // Handle cases where the script tag is not properly closed
                    break;
                }
            }

            return scripts;
        }

        internal static List<BaseFinding> ParseJavaScriptFindings(List<ScriptInfo> scripts, Microsoft.CodeAnalysis.TextDocument cshtmlFile)
        {
            var findings = new List<BaseFinding>();

            foreach (var script in scripts)
            {
                if (!script.ContainsBody)
                {
                    if (string.IsNullOrEmpty(script.Integrity))
                    {
                        BaseFinding finding;

                        if (script.IsExternal)
                            finding = new ExternalJavaScriptMissingIntegrityHash();
                        else
                            finding = new InternalJavaScriptMissingIntegrityHash();

                        finding.RootLocation = script.GetLocation(cshtmlFile.FilePath);
                        findings.Add(finding);
                    }
                }
                else
                {
                    BaseFinding finding;

                    if (string.IsNullOrEmpty(script.Nonce))
                        finding = new InlineJavaScriptWithoutNonce();
                    else
                        finding = new InlineJavaScriptWithNonce();

                    finding.RootLocation = script.GetLocation(cshtmlFile.FilePath);
                    findings.Add(finding);
                }
            }

            return findings;
        }
    }
}
