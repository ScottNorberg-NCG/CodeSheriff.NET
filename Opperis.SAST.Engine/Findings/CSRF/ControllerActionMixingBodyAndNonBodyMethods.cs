using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.CSRF
{
    internal class ControllerActionMixingBodyAndNonBodyMethods : BaseFinding
    {
        internal override Priority Priority
        {
            get
            {
                if (_priority == null)
                    _priority = Priority.MediumLow;

                return _priority;
            }
        }

        internal override string FindingText { get { return "Controller Action Has Multiple Methods"; } }

        internal override string Description { get { return "A Controller action was with multiple method attributes, such as having both an [HttpGet] and [HttpPost]. If this endpoint is intended for methods without bodies (such as GETs) only this is likely not a security concern. If it is used for methods with bodies (such as POSTs or PUTs), this can be used to bypass CSRF checks."; } }
    }
}
