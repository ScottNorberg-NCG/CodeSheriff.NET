using Microsoft.EntityFrameworkCore;
using CodeSheriff.SCA.Engine.Data;
using CodeSheriff.SCA.Engine.NVD;
using System.Net.Http.Json;
using System.Threading.RateLimiting;
using System.Web;

namespace CodeSheriff.SCA.Engine;

public class NVDLoader
{
    private const string _baseUri = "https://services.nvd.nist.gov/rest/json/";
    private const string _version = "0.0.1";
    private readonly string? _apiKey;
    private readonly HttpClient _client;
    private readonly SlidingWindowRateLimiter _rateLimiter;

    public NVDLoader(string? apiKey = null, string userAgent = $"CodeSheriff.SCA/{_version}")
    {
        SlidingWindowRateLimiterOptions rateLimiterOptions = new()
        {
            AutoReplenishment = true,
            PermitLimit = 5,
            QueueLimit = 5000,
            SegmentsPerWindow = 10,
            Window = TimeSpan.FromSeconds(30),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        };

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            rateLimiterOptions.PermitLimit = 50;
        }

        _rateLimiter = new(rateLimiterOptions);
        _client = new(new RateLimitingHandler(_rateLimiter));

        if (!string.IsNullOrWhiteSpace(userAgent)) _client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        if (!string.IsNullOrWhiteSpace(apiKey)) _client.DefaultRequestHeaders.Add("apikey", apiKey);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public List<CveInfo> GetForAssembly(string assemblyName)
    {
        var toReturn = new List<CveInfo>();
        var response = GetCves(1, 2000, assemblyName);

        foreach (var vuln in response.Vulnerabilities)
        {
            toReturn.Add(new CveInfo(vuln));
        }

        return toReturn;
    }

    public void LoadDatabase()
    {
        int count = 0;
        int currentPage = 1;
        int resultsPerPage = 1000;

        var dbContext = new ApplicationDbContext();
        //dbContext.Database.SetConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-53bc9b9d-9d6a-45d4-8429-2a2761773502;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        dbContext.Database.SetConnectionString("Server=localhost\\SQL2019;Database=TempCve;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
        dbContext.RebuildDatabase();

        do
        { 
            var startIndex = resultsPerPage * currentPage;
            var response = GetCves(startIndex, resultsPerPage, "*");
            count = response.Vulnerabilities.Count();

            foreach (var vuln in response.Vulnerabilities)
            {
                var cveInfo = new CveInfo(vuln);

                if (cveInfo.ParsedCpes.Count > 0)
                    dbContext.CveInfos.Add(cveInfo);
            }

            dbContext.SaveChanges();

            Console.WriteLine($"Finished loading page {currentPage}");
            currentPage++;
        }

        while (count > 0);
    }

    /// <summary>
    ///     Calls the API with the provided options.
    /// </summary>
    /// <param name="options">
    ///     <see cref="CvesRequestOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="CveResponse" />
    /// </returns>
    /// <exception cref="Exception">An exception is thrown if the API call fails</exception>
    /// <remarks>
    ///     To get All vulnerabilities, start by calling the API beginning with a startIndex of 0.
    ///     Successive requests should increment the startIndex by the value of resultsPerPage until
    ///     the response's startIndex has exceeded the value in totalResults.
    ///     <para>
    ///         After initial data population the last modified date parameters provide an efficient
    ///         way to update a user's local repository and stay within the API rate limits.
    ///         No more than once every two hours, automated requests should include a range where
    ///         lastModStartDate equals the time of the last record received from that and lastModEndDate
    ///         equals the current time.
    ///     </para>
    /// </remarks>
    private CveResponse GetCves(int startIndex, int resultsPerPage, string assembly)
    {
        const string cveBaseUri = $"{_baseUri}cves/2.0?";
        List<string> queryStringParams = new();

        queryStringParams.Add($"resultsPerPage={resultsPerPage}");
        queryStringParams.Add($"startIndex={startIndex}");

        if (string.IsNullOrEmpty(assembly))
            assembly = "*";

        queryStringParams.Add($"virtualMatchString=cpe:2.3:a:*:{assembly}:*");

        CveResponse? cveResponse = null;
        HttpResponseMessage response = null;

        try
        {
            response = _client.GetAsync(new Uri($"{cveBaseUri}{string.Join('&', queryStringParams)}")).Result;
            cveResponse = response.Content.ReadFromJsonAsync<CveResponse>().Result;

            if (cveResponse != null)
            {
                cveResponse.StatusCode = response.StatusCode;
                cveResponse.ReasonPhrase = response.ReasonPhrase;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new InvalidDataException(response.StatusCode.ToString());


        }
        catch (Exception e)
        {
            var a = $"{cveBaseUri}{string.Join('&', queryStringParams)}";
            Console.Write(response.RequestMessage);
            Console.Write(a);
            Console.Write(e);
        }

        return cveResponse;
    }
}
