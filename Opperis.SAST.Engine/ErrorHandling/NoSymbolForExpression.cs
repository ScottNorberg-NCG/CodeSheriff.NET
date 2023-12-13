using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.ErrorHandling;

internal class NoSymbolForExpression : BaseError
{
    internal override ErrorCategory Category => ErrorCategory.NoSymbolForExpression;

    internal NoSymbolForExpression(ExpressionSyntax syntax)
    { 
        base.CodeLocation = new Findings.SourceLocation(syntax);
    }
}
