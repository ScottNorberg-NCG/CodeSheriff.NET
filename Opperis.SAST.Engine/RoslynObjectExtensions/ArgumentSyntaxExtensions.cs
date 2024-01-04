using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions;

internal static class ArgumentSyntaxExtensions
{
    internal static bool IsOfType(this ArgumentSyntax argument, string typeName)
    {
        var underlyingType = argument.Expression.GetUnderlyingType();

        if (underlyingType == null)
            return false;

        return underlyingType.ToString().Replace("?", "") == typeName;
    }
}
