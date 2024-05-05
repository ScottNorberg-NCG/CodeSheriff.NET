using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.Data;

public class ParsedCpe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string CveId { get; set; }

    //public string CpeVersion { get; set; }
    public string Vendor { get; set; }

    [NotMapped]
    public string Part { get; set; }
    public string Product { get; set; }
    //public string Update { get; set; }
    public string ProductVersion { get; set; }
    //public string Language { get; set; }
    //public string Edition { get; set; }
    //public string TargetHardware { get; set; }
    //public string SoftwareEdition { get; set; }
    //public string Other { get; set; }

    public int? MinVersionMajor { get; set; }
    public int? MinVersionMinor { get; set; }
    public int? MinVersionRevision { get; set; }
    public bool MinVersionIncludesEqual { get; set; }

    public int? MaxVersionMajor { get; set; }
    public int? MaxVersionMinor { get; set; }
    public int? MaxVersionRevision { get; set; }
    public bool MaxVersionIncludesEqual { get; set; }

    public CveInfo CveInfo { get; set; }

    //cpe:2.3:(part):(vendor):(product):(version):(update):(edition):(language):(swEdition):(targetHw):(other)
    public static ParsedCpe Parse(string cpeString)
    {
        var cpeParts = cpeString.Split(':');

        return new ParsedCpe()
        {
            //CpeVersion = cpeParts[1],
            Part = cpeParts[2],
            Vendor = cpeParts[3],
            Product = cpeParts[4],
            ProductVersion = cpeParts[5],
            //Update = cpeParts[6],
            //Edition = cpeParts[7],
            //Language = cpeParts[8],
            //SoftwareEdition = cpeParts[9],
            //TargetHardware = cpeParts[10],
            //Other = cpeParts[11]
        };
    }

    public bool IsMatch(SoftwareVersion assemblyVersion)
    {
        if (!this.MaxVersionMajor.HasValue && !this.MinVersionMajor.HasValue)
            return false;

        return IsMinVersionMatch(assemblyVersion) && IsMaxVersionMatch(assemblyVersion);
    }

    public void SetMinVersion(string minVersion, bool includeEqual)
    { 
        var softwareVersion = new SoftwareVersion(minVersion);

        this.MinVersionMajor = softwareVersion.Major;
        this.MinVersionMinor = softwareVersion.Minor;
        this.MinVersionRevision = softwareVersion.Revision;
        this.MinVersionIncludesEqual = includeEqual;
    }

    public void SetMaxVersion(string maxVersion, bool includeEqual)
    {
        var softwareVersion = new SoftwareVersion(maxVersion);

        this.MaxVersionMajor = softwareVersion.Major;
        this.MaxVersionMinor = softwareVersion.Minor;
        this.MaxVersionRevision = softwareVersion.Revision;
        this.MaxVersionIncludesEqual = includeEqual;
    }

    private bool IsMinVersionMatch(SoftwareVersion assemblyVersion)
    {
        if (this.MinVersionMajor < assemblyVersion.Major)
            return true;
        else if (this.MinVersionMajor > assemblyVersion.Major) 
            return false;
        else if (this.MinVersionMinor < assemblyVersion.Minor)
            return true;
        else if (this.MinVersionMinor > assemblyVersion.Minor)
            return false;
        else if (this.MinVersionRevision < assemblyVersion.Revision)
            return true;
        else if (this.MinVersionRevision > assemblyVersion.Revision)
            return false;

        return this.MinVersionIncludesEqual;
    }

    private bool IsMaxVersionMatch(SoftwareVersion assemblyVersion)
    {
        if (this.MaxVersionMajor > assemblyVersion.Major)
            return true;
        else if (this.MaxVersionMajor < assemblyVersion.Major)
            return false;
        else if (this.MaxVersionMinor > assemblyVersion.Minor)
            return true;
        else if (this.MaxVersionMinor < assemblyVersion.Minor)
            return false;
        else if (this.MaxVersionRevision > assemblyVersion.Revision)
            return true;
        else if (this.MaxVersionRevision < assemblyVersion.Revision)
            return false;

        return this.MaxVersionIncludesEqual;
    }
}
