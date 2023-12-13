using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Opperis.SCA.Engine.NVD;

public class VendorComment
{
    [JsonPropertyName("organization")] public string? Organization { get; set; }

    [JsonPropertyName("comment")] public string? Comment { get; set; }

    [JsonPropertyName("lastModified")] public DateTimeOffset LastModified { get; set; }
}
