using Opperis.SCA.Engine.NVD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SCA.Engine.Data;

public class CveInfo
{
    public string CveId { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime PublishedOn { get; set; }
    public double? Severity { get; set; }
    public string? SeverityType { get; set; }

    public ICollection<CveReference> CveReferences { get; set; } = new HashSet<CveReference>();

    public ICollection<ParsedCpe> ParsedCpes { get; set; } = new HashSet<ParsedCpe>();

    public CveInfo() { }

    public CveInfo(NVDVulnerability vuln)
    {
        this.CveId = vuln.Cve.Id;
        this.PublishedOn = vuln.Cve.Published;
        this.Status = vuln.Cve.VulnStatus;
        this.Description = vuln.Cve.Descriptions.First(d => d.Lang == "en").Value;

        if (vuln.Cve.Metrics != null)
        {
            if (vuln.Cve.Metrics.CvssMetricV3 != null)
            {
                this.Severity = vuln.Cve.Metrics.CvssMetricV3.First().CvssData.BaseScore;
                this.SeverityType = "CVSS v3";
            }
            else if (vuln.Cve.Metrics.CvssMetricV2 != null)
            {
                this.Severity = vuln.Cve.Metrics.CvssMetricV2.First().CvssData.BaseScore;
                this.SeverityType = "CVSS v2";
            }
        }

        foreach (var reference in vuln.Cve.References)
        {
            var newReference = new CveReference();

            newReference.Url = reference.Url.ToString();
            newReference.Source = reference.Source;

            this.CveReferences.Add(newReference);
        }

        if (vuln.Cve.Configurations != null)
        {
            foreach (var match in vuln.Cve.Configurations.SelectMany(c => c.Nodes).SelectMany(n => n.CpeMatch).Where(m => m.Vulnerable))
            {
                var parsed = ParsedCpe.Parse(match.Criteria);

                //a = application, o = operating system, h = hardware
                if (parsed.Part == "a")
                {
                    if (!string.IsNullOrEmpty(match.VersionStartIncluding))
                    {
                        parsed.SetMinVersion(match.VersionStartIncluding, true);
                    }
                    if (!string.IsNullOrEmpty(match.VersionEndExcluding))
                    {
                        parsed.SetMaxVersion(match.VersionEndExcluding, false);
                    }
                    if (!string.IsNullOrEmpty(match.VersionEndIncluding))
                    {
                        parsed.SetMaxVersion(match.VersionEndIncluding, true);
                    }

                    if (!parsed.MaxVersionMajor.HasValue && !parsed.MinVersionMajor.HasValue)
                    {
                        parsed.SetMinVersion(parsed.ProductVersion, true);
                        parsed.SetMaxVersion(parsed.ProductVersion, true);
                    }

                    this.ParsedCpes.Add(parsed);
                }
            }
        }
    }
}
