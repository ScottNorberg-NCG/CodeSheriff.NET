using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Opperis.SCA.Engine.NVD;

public class MetricsClass
{
    [JsonPropertyName("cvssMetricV2")] public CvssMetricV2[]? CvssMetricV2 { get; set; }
    [JsonPropertyName("cvssMetricV30")] public CvssMetricV3[]? CvssMetricV3 { get; set; }
}
