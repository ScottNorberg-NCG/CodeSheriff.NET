using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.NVD;

public class CvssDataV3
{
    [JsonPropertyName("version")] public string? Version { get; set; }

    [JsonPropertyName("vectorString")] public string? VectorString { get; set; }

    [JsonPropertyName("attackVector")] public string? AttachVector { get; set; }

    [JsonPropertyName("attackComplexity")] public string? AttackComplexity { get; set; }

    [JsonPropertyName("privilegesRequired")]
    public string? PrivilegeRequired { get; set; }

    [JsonPropertyName("userInteraction")] public string? UserInteraction { get; set; }
    [JsonPropertyName("scope")] public string? Scope { get; set; }

    [JsonPropertyName("confidentialityImpact")]
    public string? ConfidentialityImpact { get; set; }

    [JsonPropertyName("integrityImpact")] public string? IntegrityImpact { get; set; }

    [JsonPropertyName("availabilityImpact")]
    public string? AvailabilityImpact { get; set; }

    [JsonPropertyName("baseScore")] public double BaseScore { get; set; }
    [JsonPropertyName("baseSeverity")] public string BaseSeverity { get; set; }
}
