using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.NVD;

public class CveResponse
{
    public HttpStatusCode StatusCode;
    public string? ReasonPhrase;

    /// <summary>Gets a value that indicates if the HTTP response was successful.</summary>
    /// <returns>
    /// <see langword="true" /> if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 200-299; otherwise, <see langword="false" />.</returns>
    public bool IsSuccessStatusCode => StatusCode >= HttpStatusCode.OK && StatusCode <= (HttpStatusCode)299;

    /// <summary>
    ///     If the value of <see cref="TotalResults" /> is greater than the value of resultsPerPage, then additional requests
    ///     are necessary to return the remaining CVE
    /// </summary>
    [JsonPropertyName("resultsPerPage")]
    public int ResultsPerPage { get; set; }

    /// <summary>
    ///     May be used in subsequent requests to identify the starting point for the next request
    /// </summary>
    [JsonPropertyName("startIndex")]
    public int StartIndex { get; set; }

    /// <summary>
    ///     Indicates the number of CVE that match the request criteria, including all parameters
    /// </summary>
    /// <seealso cref="ResultsPerPage" />
    [JsonPropertyName("totalResults")]
    public int TotalResults { get; set; }

    /// <summary>
    ///     Identify the format of the API Response.
    /// </summary>
    [JsonPropertyName("format")]
    public string Format { get; set; }

    /// <summary>
    ///     Identify the version of the API Response.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; }

    /// <summary>
    ///     Identifies when the response was generated
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    ///     Contains an array of objects equal to the number of CVE returned in the response.
    /// </summary>
    [JsonPropertyName("vulnerabilities")]
    public NVDVulnerability[] Vulnerabilities { get; set; }
}
