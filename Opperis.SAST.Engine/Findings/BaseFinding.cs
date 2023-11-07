using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.DataCleaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings
{
    internal abstract class BaseFinding
    {
        protected Priority? _priority;
        internal abstract Priority Priority { get; }
        internal abstract string FindingText { get; }
        internal abstract string Description { get; }
        internal string AdditionalInformation { get; set; } = "(None)";

        internal List<CallStack> CallStacks { get; } = new List<CallStack>();
        internal SourceLocation RootLocation { get; set; }

        internal void RedactAllByteArrays()
        {
            RootLocation.Text = StringRedactor.RedactByteArray(RootLocation.Text);

            foreach (var callStack in CallStacks) 
            {
                foreach (var location in callStack.Locations)
                { 
                    location.Text = StringRedactor.RedactByteArray(location.Text);
                }
            }
        }
    }
}
