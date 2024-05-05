using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.NuGet;

public class Item
{
    [JsonPropertyName("catalogEntry")]
    public CatalogEntry catalogEntry { get; set; }
}
