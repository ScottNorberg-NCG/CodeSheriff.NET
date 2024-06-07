using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Html.JavaScriptTags;

internal class InlineJavaScriptWithNonce : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.VeryLow;

            return _priority;
        }
    }

    public override string FindingText { get { return "Inline JavaScript Found (with Nonce)"; } }

    public override string Description { get { return "JavaScript is most secure when it is stored in external files. Inline CSS is not recommended because it makes it harder to utilize CSP headers that protect users from inline scripts and styles. With that said, nonces can reduce the risk of maliciously-inserted inline styles being added."; } }
}
