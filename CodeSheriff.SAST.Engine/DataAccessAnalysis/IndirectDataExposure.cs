using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.DataAccessAnalysis;

internal class IndirectDataExposure : DataAccessItem
{
    public IndirectDataExposure(MethodDeclarationSyntax method, ITypeSymbol containingType, string propertyName, List<CallStack> callStacks) : base(method, containingType, propertyName, callStacks)
    {
    }

    internal override Direction DataDirection => Direction.ToView;
}
