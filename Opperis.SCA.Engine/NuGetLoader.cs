using Opperis.SCA.Engine.NuGet;
using Opperis.SCA.Engine.NVD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Opperis.SCA.Engine;

public static class NuGetLoader
{
    private const string _baseUri = "https://api.nuget.org/v3/registration5-gz-semver2/{0}/index.json";

    public static List<CatalogEntry> GetNuGetInfo(string assembly)
    {
        var handler = new HttpClientHandler();
        handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("User-Agent", "Opperis SAST");
  
        var url = string.Format(_baseUri, assembly.ToLower());

        var response = client.GetAsync(new Uri(url)).Result;

        if (!response.IsSuccessStatusCode)
            return new List<CatalogEntry>();

        var contentAsString = response.Content.ReadAsStringAsync().Result;
        var nuGetContainer = JsonSerializer.Deserialize<Container>(contentAsString); //response.Content.ReadFromJsonAsync<Container>().Result;

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            throw new InvalidDataException(response.StatusCode.ToString());

        var toReturn = new List<CatalogEntry>();

        toReturn.AddRange(nuGetContainer.items.Where(i => i.items != null).SelectMany(i => i.items).Where(i => i.catalogEntry != null).Select(i => i.catalogEntry));

        if (toReturn.Count == 0)
        {
            foreach (var item in nuGetContainer.items)
            {
                var childResponse = client.GetAsync(new Uri(item.id)).Result;
                var asString = childResponse.Content.ReadAsStringAsync().Result;
                var page = JsonSerializer.Deserialize<ChildContainer>(asString); //childResponse.Content.ReadFromJsonAsync<ChildContainer>().Result;
                toReturn.AddRange(page.items.Where(i => i.catalogEntry != null).Select(i => i.catalogEntry));
            }
        }

        return toReturn;
    }
}
