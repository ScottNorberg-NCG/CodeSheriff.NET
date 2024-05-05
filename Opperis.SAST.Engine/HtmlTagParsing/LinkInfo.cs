using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.HtmlTagParsing
{
    internal class LinkInfo : BaseHtmlInfo
    {
        internal string? Nonce { get; set; }
        internal string? Integrity { get; set; }
        internal bool IsExternal { get; private set; }

        internal LinkInfo(string text)
        {
            this.Text = text;

            SetHrefRelatedProperties(text);
            SetNonce(text);
            SetIntegrity(text);
        }

        private void SetHrefRelatedProperties(string text)
        {
            string hrefPattern = GetRegexPattern("link", "href");

            Match match = Regex.Match(text, hrefPattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string href = match.Groups[2].Value;

                if (href.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || href.StartsWith("https://", StringComparison.OrdinalIgnoreCase) || href.StartsWith("//"))
                    this.IsExternal = true;
                else
                    this.IsExternal = false;
            }
        }

        private void SetNonce(string text)
        {
            string noncePattern = GetRegexPattern("link", "href");

            Match match = Regex.Match(text, noncePattern, RegexOptions.IgnoreCase);

            if (match.Success)
                this.Nonce = match.Groups[2].Value;
        }

        private void SetIntegrity(string text)
        {
            string integrityPattern = GetRegexPattern("link", "integrity");

            Match match = Regex.Match(text, integrityPattern, RegexOptions.IgnoreCase);

            if (match.Success)
                this.Integrity = match.Groups[2].Value;
        }
    }
}
