using CodeSheriff.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.ErrorHandling;

public abstract class BaseError
{
    public enum ErrorCategory
    { 
        CannotFindUnderlyingType,
        InvalidRegex,
        NoSymbolForExpression,
        NuGetProcessingError,
        UnknownForSingleFinding,
        VariableMissingIdentifier
    }

    public abstract ErrorCategory Category { get; }

    public string ErrorMessage { get; }
    public Exception? BaseException { get; protected set; }
    public SourceLocation? CodeLocation { get; protected set; }
}
