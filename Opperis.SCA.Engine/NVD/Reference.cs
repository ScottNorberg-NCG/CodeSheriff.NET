using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Opperis.SCA.Engine.NVD;

public class Reference
{
    [JsonPropertyName("url")] public Uri? Url { get; set; }

    /// <summary>
    /// Identifies the organization that provided the reference information.
    /// </summary>
    [JsonPropertyName("source")] public string? Source { get; set; }
}
