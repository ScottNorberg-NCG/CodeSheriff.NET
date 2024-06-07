using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.HtmlTagParsing;

public class ScriptInfo : BaseHtmlInfo
{
    internal string? Nonce { get; set; }
    internal string? Integrity { get; set; }
    internal bool IsExternal { get; private set; }
    internal bool ContainsBody { get; private set; }

    internal ScriptInfo(string text)
    {
        this.Text = text;

        SetSrcRelatedProperties(text);
        SetNonce(text);
        SetIntegrity(text);
    }

    private void SetSrcRelatedProperties(string text)
    {
        var src = "";

        string srcPattern = GetRegexPattern("script", "src");

        Match match = Regex.Match(text, srcPattern, RegexOptions.IgnoreCase);

        if (match.Success)
        {
            src = match.Groups[2].Value;

            if (src.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || src.StartsWith("https://", StringComparison.OrdinalIgnoreCase) || src.StartsWith("//"))
                this.IsExternal = true;
            else
                this.IsExternal = false;

            this.ContainsBody = false;
        }
        else
        {
            this.ContainsBody = true;
        }
    }

    private void SetNonce(string text)
    {
        string noncePattern = GetRegexPattern("script", "nonce");

        Match match = Regex.Match(text, noncePattern, RegexOptions.IgnoreCase);

        if (match.Success)
            this.Nonce = match.Groups[2].Value;
    }

    private void SetIntegrity(string text)
    {
        string integrityPattern = GetRegexPattern("script", "integrity");

        Match match = Regex.Match(text, integrityPattern, RegexOptions.IgnoreCase);

        if (match.Success)
            this.Integrity = match.Groups[2].Value;
    }
}
