using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Html.JavaScriptTags;

internal class ExternalJavaScriptMissingIntegrityHash : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.Low;

            return _priority;
        }
    }

    public override string FindingText { get { return "External JavaScript Used Without Integrity Hash"; } }

    public override string Description { get { return "To help prevent malicious actors from tampering with external JavaScript files, you should include an integrity hash on your script tag. This will allow the browser to block any scripts that have changed without your knowledge."; } }
}
