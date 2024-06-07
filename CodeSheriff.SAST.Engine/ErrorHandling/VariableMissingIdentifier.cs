using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.ErrorHandling;

internal class VariableMissingIdentifier : BaseError
{
    public override ErrorCategory Category => ErrorCategory.VariableMissingIdentifier;

    internal VariableMissingIdentifier(ISymbol symbol)
    {
        base.CodeLocation = new Findings.SourceLocation(symbol);
        base.BaseException = new Exception(new System.Diagnostics.StackTrace().ToString());
    }
}
