using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Http
{
    internal class CookieAddedWithoutOptions : BaseFinding
    {
        internal override Priority Priority
        {
            get
            {
                if (_priority == null)
                    _priority = Priority.Medium;

                return _priority;
            }
        }

        internal override string FindingText { get { return "Cookie created without CookieOptions set"; } }

        internal override string Description { get { return "A cookie was appended to the HTTP Response without being configured via a CookieOptions object. By default, cookies in .NET are allowed to be sent over both HTTP and HTTPS and are accessible via JavaScript, making the cookie more likely to be discovered by malicious actors."; } }

        public CookieAddedWithoutOptions(InvocationExpressionSyntax cookie)
        {
            base.RootLocation = new SourceLocation(cookie);
        }
    }
}
