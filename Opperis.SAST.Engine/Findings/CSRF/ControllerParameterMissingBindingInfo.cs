using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.CSRF
{
    internal class ControllerParameterMissingBindingInfo : BaseFinding
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

        internal override string FindingText { get { return "Controller Action Parameters Missing Binding Info"; } }

        internal override string Description { get { return "A Controller action was found with parameters that are missing binding information attributes, such as [FromForm] or [FromBody]. When these attributes are missing, attackers can send information via channels that the developer didn't intend. This can be especially problematic if the method attribute is missing and an attacker can use the method (e.g. GET or POST) of their choice."; } }
    }
}
