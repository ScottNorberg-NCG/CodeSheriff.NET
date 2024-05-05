using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Http
{
    internal class CookieAddedWithHttpOnlySetToFalse : BaseFinding
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

        internal override string FindingText { get { return "Cookie created with HttpOnly set to false"; } }

        internal override string Description { get { return "A cookie was appended to the HTTP Response and was configured so that HttpOnly was set to false. This setting allows JavaScript to access the cookie, potentially making the data available to hackers in a Cross-Site Scripting attack."; } }

        public CookieAddedWithHttpOnlySetToFalse(ExpressionSyntax cookie)
        {
            base.RootLocation = new SourceLocation(cookie);
        }
    }
}
