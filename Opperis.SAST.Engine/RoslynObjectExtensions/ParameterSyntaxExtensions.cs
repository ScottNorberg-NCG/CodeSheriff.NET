using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions;

internal static class ParameterSyntaxExtensions
{
    internal static bool HasBindingSourceInfo(this ParameterSyntax parameter)
    {
        var model = Globals.SearchForSemanticModel(parameter.SyntaxTree);
        return parameter.AttributeLists.Any(al => al.Attributes.Any(a => a.HasBindingSourceAttribute(model)));
    }
}
