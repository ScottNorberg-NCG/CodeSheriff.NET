using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Opperis.SCA.Engine.NVD;

public class Cve
{
    /// <summary>
    ///     CVE Id of the vulnerability
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }


    /// <summary>
    ///     Pontains a description of the CVE in one or more languages.
    ///     ISO 639-1:2002's two-letter language identifiers indicate the language of the description.
    ///     Spanish language translations are provided by the Spanish National Cybersecurity Institute (INCIBE).
    /// </summary>
    [JsonPropertyName("descriptions")]
    public Description[] Descriptions { get; set; }

    /// <summary>
    ///     Contains information on the CVE's impact.
    ///     If the CVE has been analyzed,
    ///     this object will contain any CVSSv2 or CVSSv3 information associated with the vulnerability.
    /// </summary>
    [JsonPropertyName("metrics")]
    public MetricsClass? Metrics { get; set; }

    /// <summary>
    ///     Contains information on specific weaknesses, considered the cause of the vulnerability.
    /// </summary>
    /// <remarks>
    ///     Please note, a CVE that is Awaiting Analysis, Undergoing Analysis,
    ///     or Rejected may not include the weaknesses object.
    /// </remarks>
    [JsonPropertyName("weaknesses")]
    public Weakness[]? Weaknesses { get; set; }

    /// <summary>
    ///     Contains the CVE applicability statements that convey which product,
    ///     or products, are associated with the vulnerability according to the NVD analysis.
    /// </summary>
    /// <remarks>
    ///     Please note, a CVE that is Awaiting Analysis, Undergoing Analysis,
    ///     or Rejected may not include the configuration object.
    /// </remarks>
    [JsonPropertyName("configurations")]
    public Configuration[]? Configurations { get; set; }

    /// <summary>
    ///     ontains supplemental information relevant to the vulnerability,
    ///     and may include details that are not present in the CVE Description.
    ///     Each reference within this object provides one or more resource tags
    ///     (e.g., third-party advisory, vendor advisory, technical paper, press/media, VDB entries).
    ///     Resource tags are designed to categorize the type of information each reference contains.
    /// </summary>
    [JsonPropertyName("references")]
    public Reference[]? References { get; set; }

    /// <summary>
    ///     Contains any Official Vendor Comment for the CVE.
    ///     NVD provides a service whereby organizations can submit Official Vendor Comments for CVE associated with their
    ///     products.
    ///     Organizations can use the service in a variety of ways. For example, they can provide configuration and remediation
    ///     guidance,
    ///     clarify vulnerability applicability, provide deeper vulnerability analysis, dispute third party vulnerability
    ///     information,
    ///     and explain vulnerability impact. Official Vendor Comments can be submitted to the NVD by email at nvd@nist.gov.
    ///     More information is provided on the vendor comments page.
    /// </summary>
    [JsonPropertyName("vendorComments")]
    public VendorComment[]? VendorComments { get; set; }

    /// <summary>
    ///     An identifier for the source of the CVE
    /// </summary>
    [JsonPropertyName("sourceIdentifier")]
    public string SourceIdentifier { get; set; }

    /// <summary>
    ///     The date and time that the CVE was published to the NVD
    /// </summary>
    [JsonPropertyName("published")]
    public DateTime Published { get; set; }

    /// <summary>
    ///     The date and time that the CVE was last modified
    /// </summary>
    [JsonPropertyName("lastModified")]
    public DateTime LastModified { get; set; }

    /// <summary>
    ///     The CVE's status in the NVD
    /// </summary>
    [JsonPropertyName("vulnStatus")]
    public string VulnStatus { get; set; }

    /// <summary>
    ///     Provides additional context to help understand the vulnerability or its analysis
    /// </summary>
    [JsonPropertyName("evaluatorComment")]
    public string? evaluatorComment { get; set; }

    /// <summary>
    ///     Provides additional context to help understand the vulnerability or its analysis
    /// </summary>
    [JsonPropertyName("evaluatorImpact")]
    public string? evaluatorImpact { get; set; }

    /// <summary>
    ///     Provides additional context to help understand the vulnerability or its analysis
    /// </summary>
    [JsonPropertyName("evaluatorSolution")]
    public string? evaluatorSolution { get; set; }

    [JsonPropertyName("cisaExploitAdd")] public string? cisaExploitAdd { get; set; }

    /// <summary>
    ///     Indicates the date by which all federal civilian executive branch (FCEB)
    ///     agencies are required to complete the <see cref="cisaRequiredAction" /> under
    ///     Binding Operational Directive (BOD) 22-01,
    ///     Reducing the Significant Risk of Known Exploited Vulnerabilities
    /// </summary>
    /// <remarks>
    ///     Although not bound by BOD 22-01, every organization,
    ///     including those in state, local, tribal, and territorial (SLTT) governments
    ///     and private industry can significantly strengthen their security and resilience
    ///     posture by prioritizing the remediation of the vulnerabilities listed in
    ///     the KEV catalog as well.
    /// </remarks>
    [JsonPropertyName("cisaActionDue")]
    public string? cisaActionDue { get; set; }

    [JsonPropertyName("cisaRequiredAction")]
    public string? cisaRequiredAction { get; set; }

    [JsonPropertyName("cisaVulnerabilityName")]
    public string? cisaVulnerabilityName { get; set; }
}
