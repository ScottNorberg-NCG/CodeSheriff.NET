using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.XSS
{
    internal class BindObjectForOtherMethodUsedInHtmlRaw : BaseFinding
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

        internal override string FindingText { get { return "Possible Cross-Site Scripting - Razor Page [BindProperty] method used in Html.Raw()"; } }

        internal override string Description { get { return "The scanner found an object that was used as an input object for a method other than GET used in a call to Html.Raw(). If an attacker sends a script in the parameter used in Html.Raw(), this will result in a Reflected Cross-Site Scripting vulnerability."; } }
    }
}
