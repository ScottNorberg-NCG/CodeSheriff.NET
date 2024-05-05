using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Html.JavaScriptTags
{
    internal class InlineJavaScriptWithoutNonce : BaseFinding
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

        internal override string FindingText { get { return "Inline Script Tag Found"; } }

        internal override string Description { get { return "JavaScript is most secure when it is stored in external files. Inline JavaScript is not recommended because it makes it harder to utilize CSP headers that protect users from inline scripts and styles."; } }
    }
}
