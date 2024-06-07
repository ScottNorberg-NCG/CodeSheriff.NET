using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Http;

internal class CookieAddedWithDefaultOptions : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.Medium;

            return _priority;
        }
    }

    public override string FindingText { get { return "Cookie created with the default CookieOptions set"; } }

    public override string Description { get { return "A cookie was appended to the HTTP Response with being configured via a default CookieOptions object. By default, cookies in .NET are allowed to be sent over both HTTP and HTTPS and are accessible via JavaScript, making the cookie more likely to be discovered by malicious actors."; } }

    public CookieAddedWithDefaultOptions(InvocationExpressionSyntax cookie)
    {
        base.RootLocation = new SourceLocation(cookie);
    }
}
