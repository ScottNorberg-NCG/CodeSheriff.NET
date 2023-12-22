using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.Cryptography
{
    internal class RSAWithInadequateKeyLength : BaseFinding
    {
        internal override Priority Priority => Priority.Medium;

        internal override string FindingText => "An instance of RSA was found with an inadequate key length";

        internal override string Description => "RSA allows for many different key lengths, but shorter key lengths are easier for criminals to break. Key lengths of at least 2048 bits are recommended.";

        public RSAWithInadequateKeyLength(ObjectCreationExpressionSyntax syntax)
        {
            this.RootLocation = new SourceLocation(syntax);
        }
    }
}
