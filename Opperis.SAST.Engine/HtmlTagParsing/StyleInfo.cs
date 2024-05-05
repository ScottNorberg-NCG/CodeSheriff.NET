using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.HtmlTagParsing
{
    internal class StyleInfo : BaseHtmlInfo
    {
        internal string? Nonce { get; set; }

        internal StyleInfo(string text)
        {
            this.Text = text;

            SetNonce(text);
        }

        private void SetNonce(string text)
        {
            string noncePattern = GetRegexPattern("style", "nonce");

            Match match = Regex.Match(text, noncePattern, RegexOptions.IgnoreCase);

            if (match.Success)
                this.Nonce = match.Groups[2].Value;
        }
    }
}
