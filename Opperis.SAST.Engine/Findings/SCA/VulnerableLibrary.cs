using Opperis.SCA.Engine.NVD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.SCA;

internal class VulnerableLibrary : BaseFinding
{
    private string? _nuGetPriority;
    internal override Priority Priority
    {
        get
        { 
            if (this._nuGetPriority == null) 
            { 
                switch (this._nuGetPriority) 
                {
                    case "3":
                        return Priority.VeryHigh;
                    case "2":
                        return Priority.High;
                    case "1":
                        return Priority.Medium;
                    case "0":
                        return Priority.Low;
                    default:
                        throw new NotImplementedException($"Cannot find priority for NuGetPriority {_nuGetPriority}");
                }
            }
            else
                throw new NotImplementedException($"Cannot determine priority");
        }
    }

    internal override string FindingText => "Vulnerable Library Found";

    private string _description;
    internal override string Description => _description;

    internal VulnerableLibrary(string priority, string libraryName, List<string> projects)
    {
        _nuGetPriority = priority;
        _description = $"Library: {libraryName}";
        AdditionalInformation = $"Library referenced in: {string.Join(", ", projects.ToArray())}";
    }
}
