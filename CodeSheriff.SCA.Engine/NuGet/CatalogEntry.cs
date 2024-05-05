using Microsoft.CodeAnalysis.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.NuGet;

public class CatalogEntry
{
    [JsonPropertyName("@id")]
    public string id { get; set; }

    [JsonPropertyName("version")]
    public string version { get; set; } = string.Empty;

    [JsonPropertyName("vulnerabilities")]
    public List<Vulnerability> vulnerabilities { get; set; } = new List<Vulnerability>();

    private SoftwareVersion _calculatedVersion;
    public SoftwareVersion CalculatedVersion
    {
        get
        { 
            if (_calculatedVersion == null) 
                _calculatedVersion = new SoftwareVersion(version);

            return _calculatedVersion;
        }
    }

    public bool IsBeta
    {
        get
        {
            int temp;
            var versionParts = version.Split('.');

            foreach (var part in versionParts) 
            {
                if (!int.TryParse(part, out temp))
                    return true;
            }

            return false;
        }
    }
}
