using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.DataAccessAnalysis;

internal class DataInView : DataAccessItem
{
    public DataInView(MethodDeclarationSyntax method, ITypeSymbol containingType, string propertyName, List<CallStack> callStacks) : base(method, containingType, propertyName, callStacks)
    {
    }

    internal override Direction DataDirection => Direction.ToView;
}
