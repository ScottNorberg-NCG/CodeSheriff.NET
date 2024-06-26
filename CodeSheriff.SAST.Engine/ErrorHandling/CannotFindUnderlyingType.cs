﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.ErrorHandling;

internal class CannotFindUnderlyingType : BaseError
{
    public override ErrorCategory Category => ErrorCategory.CannotFindUnderlyingType;

    internal CannotFindUnderlyingType(ExpressionSyntax syntax)
    {
        base.CodeLocation = new Findings.SourceLocation(syntax);
        base.BaseException = new Exception(new System.Diagnostics.StackTrace().ToString());
    }
}
