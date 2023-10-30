using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine
{
    internal static class Globals
    {
        public static Solution Solution { get; set; }
        public static Compilation Compilation { get; set; }
    }
}
