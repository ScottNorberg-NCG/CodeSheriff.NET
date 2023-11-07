using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.Html.LinkTags
{
    internal class ExternalCssMissingIntegrityHash : BaseFinding
    {
        internal override Priority Priority
        {
            get
            {
                if (_priority == null)
                    _priority = Priority.Low;

                return _priority;
            }
        }

        internal override string FindingText { get { return "External CSS Link Missing Integrity Hash"; } }

        internal override string Description { get { return "When you link to CSS files, you should include an integrity hash to ensure that if the file was tampered with maliciously that it will not be loaded by the browser, ensuring that an attacker cannot use it to attack your users."; } }
    }
}
