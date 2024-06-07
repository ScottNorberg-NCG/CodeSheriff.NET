using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.SCA;

internal class OutdatedLibrary : BaseFinding
{
    public override Priority Priority => Priority.Info;

    public override string FindingText => "Outdated library found";

    private string _description;
    public override string Description => _description;

    internal OutdatedLibrary(string libraryName, List<string> projects)
    { 
        _description = $"Library: {libraryName}";
        AdditionalInformation = $"Library referenced in: {string.Join(", ", projects.ToArray())}";
    }
}
