using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.XSS;

internal class HtmlHelperPropertyFromOtherControllerParameter : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.Medium;

            return _priority;
        }
    }

    public override string FindingText { get { return "Possible Cross-Site Scripting - Controller method parameter used in IHtmlHelper method"; } }

    public override string Description { get { return "The scanner found an object that was used as an input object used in a call to an IHtmlHelper extension method. If an attacker sends a script in the parameter used in Html.Raw(), this will result in a Reflected Cross-Site Scripting vulnerability."; } }
}
