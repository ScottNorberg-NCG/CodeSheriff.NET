using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.SCA;

internal class OutdatedLibrary : BaseFinding
{
    internal override Priority Priority => Priority.Info;

    internal override string FindingText => "Outdated library found";

    private string _description;
    internal override string Description => _description;

    internal OutdatedLibrary(string libraryName, List<string> projects)
    { 
        _description = $"Library: {libraryName}";
        AdditionalInformation = $"Library referenced in: {string.Join(", ", projects.ToArray())}";
    }
}
