using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.NVD;

public class CvssDataV2
{
    [JsonPropertyName("version")] public string? Version { get; set; }

    [JsonPropertyName("vectorString")] public string? VectorString { get; set; }

    [JsonPropertyName("accessVector")] public string? AccessVector { get; set; }

    [JsonPropertyName("accessComplexity")] public string? AccessComplexity { get; set; }

    [JsonPropertyName("authentication")] public string? Authentication { get; set; }

    [JsonPropertyName("confidentialityImpact")]
    public string? ConfidentialityImpact { get; set; }

    [JsonPropertyName("integrityImpact")] public string? IntegrityImpact { get; set; }

    [JsonPropertyName("availabilityImpact")]
    public string? AvailabilityImpact { get; set; }

    [JsonPropertyName("baseScore")] public double BaseScore { get; set; }
}
