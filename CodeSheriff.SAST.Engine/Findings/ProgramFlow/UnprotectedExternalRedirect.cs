﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.ProgramFlow;

internal class UnprotectedExternalRedirect : BaseFinding
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

    public override string FindingText { get { return "External redirect used"; } }

    public override string Description { get { return "We found a variable that may have been sent by an untrusted source (like the user) sent to a call to Redirect(). Since Redirect() may redirect users to external sites, this may be abused in phishing attacks."; } }
}
