using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.DataAccessAnalysis;

internal class DataObjectExposedToUI : DataAccessItem
{
    public DataObjectExposedToUI(MethodDeclarationSyntax method, ITypeSymbol containingType, string propertyName, List<CallStack> callStacks) : base(method, containingType, propertyName, callStacks)
    {
    }

    public override Direction DataDirection => Direction.ToUI;
}
