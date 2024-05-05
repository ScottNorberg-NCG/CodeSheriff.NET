using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.CompiledCSHtmlParsing
{
    internal class MethodCandidate
    {
        public string? Area { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
    }
}
