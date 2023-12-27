using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.Resources;

internal class UnprotectedPhysicalFileResultPath : BaseFinding
{
    internal override Priority Priority => Priority.Medium;

    internal override string FindingText => "A PhysicalFileResult ";

    internal override string Description => throw new NotImplementedException();
}
