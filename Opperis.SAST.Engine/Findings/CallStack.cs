using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings
{
    internal class CallStack
    {
        public List<SourceLocation> Locations { get; set; } = new List<SourceLocation>();
    }
}
