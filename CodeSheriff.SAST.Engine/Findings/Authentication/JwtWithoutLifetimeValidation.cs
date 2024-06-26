﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Authentication;

internal class JwtWithoutLifetimeValidation : BaseFinding
{
    public override Priority Priority => Priority.MediumLow;

    public override string FindingText => "JWTs are configured so that token lifetimes are not validated";

    public override string Description => "Setting token lifetimes limits the damage an attacker with a stolen token can do. If a token is stolen, an attacker may be able to use that token indefinitely.";

    internal JwtWithoutLifetimeValidation(AssignmentExpressionSyntax assignment)
    {
        base.RootLocation = new SourceLocation(assignment);
    }
}
