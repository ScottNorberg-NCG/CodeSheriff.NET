using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Html.LinkTags;

internal class InlineCssWithNonce : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.Info;

            return _priority;
        }
    }

    public override string FindingText { get { return "Inline Style Tag Found (with Nonce)"; } }

    public override string Description { get { return "CSS is most secure when it is stored in external files. Inline CSS is not recommended because it makes it harder to utilize CSP headers that protect users from inline scripts and styles. With that said, nonces can reduce the risk of maliciously-inserted inline styles being added."; } }
}
