﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.XSS;

internal class HtmlRawPropertyFromGetControllerParameter : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.VeryHigh;

            return _priority;
        }
    }

    public override string FindingText { get { return "Possible Cross-Site Scripting - Html.Raw() value from Controller parameter"; } }

    public override string Description { get { return "The scanner found a property that originated from a Controller parameter was used in a call to Html.Raw(). This likely results in a Reflected Cross-Site Scripting vulnerability. Because the Controller method allows GETs, this may be used for phishing attempts in ways that POSTs (or similar) cannot be."; } }
}
