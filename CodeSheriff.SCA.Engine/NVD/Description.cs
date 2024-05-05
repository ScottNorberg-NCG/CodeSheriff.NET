using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.NVD;

public class Description
{
    [JsonPropertyName("lang")] public string Lang { get; set; }

    [JsonPropertyName("value")] public string Value { get; set; }
}
