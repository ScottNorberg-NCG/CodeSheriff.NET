using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.NVD;

public class Weakness
{
    [JsonPropertyName("source")] public string? Source { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("description")] public Description[]? Descriptions { get; set; }
}
