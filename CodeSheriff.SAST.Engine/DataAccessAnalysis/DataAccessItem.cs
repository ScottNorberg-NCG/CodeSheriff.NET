using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.Findings;
using CodeSheriff.SAST.Engine.RoslynObjectExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.DataAccessAnalysis;

public abstract class DataAccessItem
{
    public enum Direction
    { 
        ToUI,
        ToView,
        ToDatabase
    }

    public abstract Direction DataDirection { get; }

    internal MethodDeclarationSyntax ContainingMethod { get; set; }
    public string ContainingType { get; set; }
    public string PropertyName { get; set; }
    public bool IsAuthorizedAccess { get; private set; }
    public List<string> Roles { get; private set; }
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
