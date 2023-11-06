using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.XSS
{
    internal class HtmlHelperPropertyFromGetControllerParameter : BaseFinding
    {
        internal override Priority Priority
        {
            get
            {
                if (_priority == null)
                    _priority = Priority.VeryHigh;

                return _priority;
            }
        }

        internal override string FindingText { get { return "Possible Cross-Site Scripting - IHtmlHelper value from Controller parameter"; } }

        internal override string Description { get { return "The scanner found a property that originated from a Controller parameter was used in a call to an IHtmlHelper extension method. This likely results in a Reflected Cross-Site Scripting vulnerability. Because the Controller method allows GETs, this may be used for phishing attempts in ways that POSTs (or similar) cannot be."; } }
    }
}
