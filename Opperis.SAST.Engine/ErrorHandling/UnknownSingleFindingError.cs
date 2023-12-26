using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.ErrorHandling;

internal class UnknownSingleFindingError : BaseError
{
    private Exception ex;

    internal override ErrorCategory Category => ErrorCategory.UnknownForSingleFinding;

    internal UnknownSingleFindingError(ISymbol symbol, Exception exception)
    {
        base.CodeLocation = new Findings.SourceLocation(symbol);
        base.BaseException = exception;
    }

    internal UnknownSingleFindingError(ExpressionSyntax syntax, Exception exception)
    {
        base.CodeLocation = new Findings.SourceLocation(syntax);
        base.BaseException = exception;
    }

    internal UnknownSingleFindingError(MethodDeclarationSyntax syntax, Exception exception)
    {
        base.CodeLocation = new Findings.SourceLocation(syntax);
        base.BaseException = exception;
    }

    public UnknownSingleFindingError(ClassDeclarationSyntax classDeclaration, Exception exception)
    {
        base.CodeLocation = new Findings.SourceLocation(classDeclaration);
        base.BaseException = exception;
    }
}
