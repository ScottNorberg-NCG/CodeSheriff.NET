using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine
{
    internal static class StringExtensions
    {
        internal static bool In(this string str, params string[] strArr)
        {
            foreach (var s in strArr)
            {
                if (s == str)
                    return true;
            }

            return false;
        }

        internal static bool In(this string str, List<string> strArr)
        {
            foreach (var s in strArr)
            {
                if (s == str)
                    return true;
            }

            return false;
        }
    }
}
