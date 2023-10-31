using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings
{
    internal abstract class BaseFinding
    {
        internal abstract Priority Priority { get; }
        internal abstract string FindingText { get; }
        internal abstract string Description { get; }
        internal string AdditionalInformation { get; set; } = "(None)";

        internal List<CallStack> CallStacks { get; } = new List<CallStack>();
        internal SourceLocation RootLocation { get; set; }
    }
}
