using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeSheriff.SCA.Engine.NVD;

public class Configuration
{
    [JsonPropertyName("nodes")] public Node[] Nodes { get; set; }
}
