using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.XSS
{
    internal class BindObjectForOtherUsedInHtmlRaw : BaseFinding
    {
        internal override Priority Priority
        {
            get
            {
                if (_priority == null)
                    _priority = Priority.Medium;

                return _priority;
            }
        }

        internal override string FindingText { get { return "Possible Cross-Site Scripting - Controller method used in Html.Raw()"; } }

        internal override string Description { get { return "The scanner found an object that was used as an input object used in a call to Html.Raw(). If an attacker sends a script in the parameter used in Html.Raw(), this will result in a Reflected Cross-Site Scripting vulnerability."; } }
    }
}
