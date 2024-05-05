﻿using CodeSheriff.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.ErrorHandling;

internal abstract class BaseError
{
    internal enum ErrorCategory
    { 
        CannotFindUnderlyingType,
        InvalidRegex,
        NoSymbolForExpression,
        NuGetProcessingError,
        UnknownForSingleFinding,
        VariableMissingIdentifier
    }

    internal abstract ErrorCategory Category { get; }

    internal string ErrorMessage { get; }
    public Exception? BaseException { get; protected set; }
    public SourceLocation? CodeLocation { get; protected set; }
}
