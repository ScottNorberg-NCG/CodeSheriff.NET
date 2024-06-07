using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Resources;

internal class UnprotectedVirtualFileResultPath : BaseFinding
{
    public override Priority Priority => Priority.Low;

    public override string FindingText => "A VirtualFileResult was found whose path includes user input.";

    public override string Description => "Since the path includes user input, the VirtualFileResult may be hijacked to include arbitrary files from the wwwroot folder.";
}
