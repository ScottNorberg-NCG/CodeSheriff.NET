using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.CSRF
{
    internal class RequestWithBodyMissingCsrfProtection : BaseFinding
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

        internal override string FindingText { get { return "Missing CSRF Protection"; } }

        internal override string Description { get { return "A Controller action was found without CSRF protection, such as a [ValidateAntiforgeryToken] attribute on either the Controller class or the Action method."; } }
    }
}
