using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Resources;

internal class UnprotectedPhysicalFileResultPath : BaseFinding
{
    public override Priority Priority => Priority.Medium;

    public override string FindingText => "A PhysicalFileResult was found whose path includes user input.";

    public override string Description => "Since the path includes user input, the PhysicalFileResult may be hijacked to include arbitrary files from elsewhere, including sensitive configuration or operating system files.";
}
