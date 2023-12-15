using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Trufflehog;

public class Result
{
    public string SourceName { get; set; }
    public string DetectorName { get; set; }
    public bool Verified { get; set; }
    public string Redacted { get; set; }
    public Metadata SourceMetadata { get; set; }
}
