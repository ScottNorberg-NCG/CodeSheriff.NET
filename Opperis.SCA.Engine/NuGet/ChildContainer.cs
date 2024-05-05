using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.NuGet;

public class ChildContainer
{
    [JsonPropertyName("@id")]
    public string id { get; set; }

    [JsonPropertyName("items")]
    public List<Item> items { get; set; }
}

