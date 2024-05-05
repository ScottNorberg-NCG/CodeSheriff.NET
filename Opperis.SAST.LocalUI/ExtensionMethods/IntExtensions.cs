using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.LocalUI.ExtensionMethods;

internal static class IntExtensions
{
    internal static string AsPercentage(this int value, int denominator)
    {
        if (value == 0)
            return "0%";
        else
            return ((float)value / (float)denominator * 100.0).ToString("##.#\\%");
    }
}
