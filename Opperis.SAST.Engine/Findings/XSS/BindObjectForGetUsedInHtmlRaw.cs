using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.XSS
{
    internal class BindObjectForGetUsedInHtmlRaw : BaseFinding
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

        internal override string FindingText { get { return "Possible Cross-Site Scripting - Razor Page [BindProperty] method used in Html.Raw()"; } }

        internal override string Description { get { return "The scanner found an object that was used as an input object for a GET used in a call to Html.Raw(). If an attacker sends a script in the parameter used in Html.Raw(), this will result in a Reflected Cross-Site Scripting vulnerability. The fact that this method allows GETs makes the likelihood of a Reflected Cross-Site Scripting attack easier to exploit."; } }
    }
}
