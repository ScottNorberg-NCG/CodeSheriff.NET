using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Opperis.SAST.Engine.Findings;
using Opperis.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.DataAccessAnalysis;

internal abstract class DataAccessItem
{
    internal enum Direction
    { 
        ToUI,
        ToView,
        ToDatabase
    }

    internal abstract Direction DataDirection { get; }

    internal MethodDeclarationSyntax ContainingMethod { get; set; }
    internal string ContainingType { get; set; }
    internal string PropertyName { get; set; }
    internal bool IsAuthorizedAccess { get; private set; }
    internal List<string> Roles { get; private set; }
    internal readonly List<CallStack> DataSourceCallStacks;

    public DataAccessItem(MethodDeclarationSyntax method, ITypeSymbol containingType, string propertyName, List<CallStack> dataSourceCallStacks)
    {
        this.ContainingMethod = method;
        this.ContainingType = containingType.ToString().Replace("?", "");
        this.PropertyName = propertyName;
        this.DataSourceCallStacks = dataSourceCallStacks;

        if (method.IsUIProcessor()) 
        {
            var methodAsSymbol = method.ToSymbol() as IMethodSymbol;

            if (methodAsSymbol != null) 
            {
                var authorizedRoles = methodAsSymbol.GetRoles();

                if (authorizedRoles != null)
                {
                    IsAuthorizedAccess = true;
                    Roles = authorizedRoles;
                }
            }
        }
    }
}
