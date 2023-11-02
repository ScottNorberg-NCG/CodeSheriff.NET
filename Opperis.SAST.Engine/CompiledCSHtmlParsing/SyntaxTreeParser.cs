using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.CompiledCSHtmlParsing
{
    internal static class SyntaxTreeParser
    {
        internal static MethodCandidate Parse(this string syntaxTree)
        {
            var metadata = syntaxTree.Split('\n')[0];
            var firstLineParts = metadata.Split(" ");

            if (firstLineParts[0] != "#pragma")
                throw new NotImplementedException($"Cannot find file info for {metadata}");

            if (firstLineParts[1] != "checksum")
                throw new NotImplementedException($"Cannot find file info for {metadata}");

            var filePath = firstLineParts[2].Trim('"');
            var folders = filePath.Split("\\");

            var methodCandidate = new MethodCandidate();
            methodCandidate.Method = folders[folders.Length - 1].Replace(".cshtml", "");
            methodCandidate.Controller = folders[folders.Length - 2];

            if (folders[folders.Length - 3] != "Views")
                methodCandidate.Area = folders[folders.Length - 3];

            return methodCandidate;
        }
    }
}
