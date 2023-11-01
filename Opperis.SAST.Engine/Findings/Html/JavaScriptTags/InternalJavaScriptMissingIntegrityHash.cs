using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.Html.JavaScriptTags
{
    internal class InternalJavaScriptMissingIntegrityHash : BaseFinding
    {
        internal override Priority Priority
        {
            get
            {
                if (_priority == null)
                    _priority = Priority.VeryLow;

                return _priority;
            }
        }

        internal override string FindingText { get { return "Internal JavaScript Used Without Integrity Hash"; } }

        internal override string Description { get { return "To help prevent malicious actors from tampering with JavaScript files, you should include an integrity hash on your script tag. This will allow the browser to block any scripts that have changed without your knowledge."; } }
    }
}
