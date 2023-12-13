using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SCA
{
    internal static class AssemblyIdentityExtensions
    {
        public static string GetUniqueIdentifier(this AssemblyIdentity id)
        {
            return $"{id.Name}|{id.Version.Major}.{id.Version.Minor}.{id.Version.Revision}";
        }
    }
}
