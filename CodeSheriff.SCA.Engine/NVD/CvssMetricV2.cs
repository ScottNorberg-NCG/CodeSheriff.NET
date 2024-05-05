using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.NVD;

public class CvssMetricV2
{
    [JsonPropertyName("source")] public string? Source { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("cvssData")] public CvssDataV2? CvssData { get; set; }

    [JsonPropertyName("baseSeverity")] public string? BaseSeverity { get; set; }

    [JsonPropertyName("exploitabilityScore")]
    public double ExploitabilityScore { get; set; }

    [JsonPropertyName("impactScore")] public double ImpactScore { get; set; }

    [JsonPropertyName("acInsufInfo")] public bool AcInsufInfo { get; set; }

    [JsonPropertyName("obtainAllPrivilege")]
    public bool ObtainAllPrivilege { get; set; }

    [JsonPropertyName("obtainUserPrivilege")]
    public bool ObtainUserPrivilege { get; set; }

    [JsonPropertyName("obtainOtherPrivilege")]
    public bool ObtainOtherPrivilege { get; set; }

    [JsonPropertyName("userInteractionRequired")]
    public bool UserInteractionRequired { get; set; }
}
