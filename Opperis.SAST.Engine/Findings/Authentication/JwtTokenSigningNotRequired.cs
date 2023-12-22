using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.Authentication;

internal class JwtTokenSigningNotRequired : BaseFinding
{
    internal override Priority Priority => Priority.Medium;

    internal override string FindingText => "JWTs configured to allow unsigned tokens";

    internal override string Description => "Setting this to false allows attackers to tamper or forge tokens, leading to unauthorized access and worse.";

    internal JwtTokenSigningNotRequired(AssignmentExpressionSyntax assignment)
    {
        base.RootLocation = new SourceLocation(assignment);
    }
}
