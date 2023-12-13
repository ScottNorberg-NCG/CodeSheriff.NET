using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Opperis.SCA.Engine.NVD;

public class Node
{
    [JsonPropertyName("operator")] public string? Operator { get; set; }

    [JsonPropertyName("negate")] public bool Negate { get; set; }

    /// <summary>
    ///     Contains the CPE Match Criteria, the criteria's unique identifier,
    ///     and a statement of whether the criteria is vulnerable.
    /// </summary>
    [JsonPropertyName("cpeMatch")]
    public CpeMatch[] CpeMatch { get; set; }
}
