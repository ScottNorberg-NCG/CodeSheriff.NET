using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Authentication;

internal class JwtMissingExpirationTime : BaseFinding
{
    internal override Priority Priority => Priority.Medium;

    internal override string FindingText => "JWTs are configured without an expiration time";

    internal override string Description => "This eliminates the mandatory check for token expiration, making it possible for expired or revoked tokens to be accepted, leading to potential security vulnerabilities such as unauthorized access, data breaches, and reuse of invalidated tokens.";

    internal JwtMissingExpirationTime(AssignmentExpressionSyntax assignment)
    { 
        base.RootLocation = new SourceLocation(assignment);
    }
}
