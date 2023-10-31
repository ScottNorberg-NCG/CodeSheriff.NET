using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.DataCleaning
{
    internal static class StringRedactor
    {
        internal static string RedactByteArray(string original)
        {
            string pattern = "\\{[^}]*\\}";
            return Regex.Replace(original, pattern, "{REDACTED}");
        }
    }
}
