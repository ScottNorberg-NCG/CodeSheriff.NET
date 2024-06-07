using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.CSRF;

internal class ControllerActionMissingVerbAttribute : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.MediumLow;

            return _priority;
        }
    }

    public override string FindingText { get { return "Controller Action Is Missing Method Attribute"; } }

    public override string Description { get { return "A Controller action was found without an attribute, such as [HttpGet] or [HttpPost], limiting the methods that could be used. If this endpoint is intended for methods without bodies (such as GETs) only this is likely not a security concern. If it is used for methods with bodies (such as POSTs or PUTs), this can be used to bypass CSRF checks."; } }
}
