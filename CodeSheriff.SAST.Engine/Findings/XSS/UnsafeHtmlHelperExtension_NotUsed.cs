using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.XSS
{
    internal class UnsafeHtmlHelperExtension : BaseFinding
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

        internal override string FindingText { get { return "Possible Cross-Site Scripting via HtmlHelper extension method"; } }

        internal override string Description { get { return "The scanner found an HtmlHelper method without any HTML encoders. This may lead to Cross-Site Scripting vulnerabilities."; } }
    }
}
