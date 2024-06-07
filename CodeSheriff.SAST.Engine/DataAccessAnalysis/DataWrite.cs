using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.DataAccessAnalysis;

internal class DataWrite : DataAccessItem
{
    public DataWrite(MethodDeclarationSyntax method, ITypeSymbol containingType, string propertyName, List<CallStack> dataSourceCallStacks) : base(method, containingType, propertyName, dataSourceCallStacks)
    {
    }

    public override Direction DataDirection => Direction.ToDatabase;
}
