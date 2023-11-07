using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.Http
{
    internal class CookieAddedWithSecureNotSet : BaseFinding
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

        internal override string FindingText { get { return "Cookie created with the default Secure value"; } }

        internal override string Description { get { return "A cookie was appended to the HTTP Response and was configured so it used the default Secure value of false. This setting allows the browser to send the cookie via HTTP (vs. HTTPS) requests, making it easier for this to be stolen via a Machine in the Middle attack."; } }

        public CookieAddedWithSecureNotSet(ExpressionSyntax cookie)
        {
            base.RootLocation = new SourceLocation(cookie);
        }
    }
}
