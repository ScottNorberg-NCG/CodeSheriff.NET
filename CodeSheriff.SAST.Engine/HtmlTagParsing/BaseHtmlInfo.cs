using CodeSheriff.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.HtmlTagParsing;

public class BaseHtmlInfo
{
    internal int LineNumberStart { get; set; }
    internal string Text { get; set; }

    internal SourceLocation GetLocation(string filePath)
    {
        var location = new SourceLocation();
        location.FilePath = filePath;
        location.LineNumber = LineNumberStart;
        location.Text = Text;
        location.LocationType = SourceLocation.SyntaxType.CshtmlFile;

        return location;
    }

    protected string GetRegexPattern(string tag, string attribute)
    {
        return @"<" + tag + @"[^>]*\s+" + attribute + @"=([""']?)((?:(?!\1)\S)*)(\1|\s|>)";
    }
}
