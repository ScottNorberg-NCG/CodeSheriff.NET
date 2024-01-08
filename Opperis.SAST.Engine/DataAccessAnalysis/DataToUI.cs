using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.DataAccessAnalysis;

internal class DataToUI : DataAccessItem
{
    public DataToUI(MethodDeclarationSyntax method, ITypeSymbol containingType, string propertyName) : base(method, containingType, propertyName)
    {
    }

    internal override Direction DataDirection => Direction.ToUI;
}
