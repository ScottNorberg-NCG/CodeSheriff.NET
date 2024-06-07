using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Authentication;

internal class JwtTokenSigningNotRequired : BaseFinding
{
    public override Priority Priority => Priority.Medium;

    public override string FindingText => "JWTs configured to allow unsigned tokens";

    public override string Description => "Setting this to false allows attackers to tamper or forge tokens, leading to unauthorized access and worse.";

    internal JwtTokenSigningNotRequired(AssignmentExpressionSyntax assignment)
    {
        base.RootLocation = new SourceLocation(assignment);
    }
}
