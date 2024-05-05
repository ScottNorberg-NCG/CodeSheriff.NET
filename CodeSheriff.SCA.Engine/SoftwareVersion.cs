using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine;

public class SoftwareVersion : IComparable<SoftwareVersion>
{
    public int? Major { get; private set; }
    public int? Minor { get; private set; }
    public int? Revision { get; private set; }

    public SoftwareVersion(string? version)
    {
        string[] splitVersion = Array.Empty<string>();

        if (!string.IsNullOrEmpty(version))
            splitVersion = version.ToString().Split('.');

        if (splitVersion.Length > 0)
            Major = ParseAsInt(splitVersion[0]);

        if (splitVersion.Length > 1)
            Minor = ParseAsInt(splitVersion[1]);

        if (splitVersion.Length > 2)
            Revision = ParseAsInt(splitVersion[2]);
    }

    public SoftwareVersion(Version version)
    {
        this.Major = version.Major; 
        this.Minor = version.Minor; 
        this.Revision = version.Revision;
    }

    private static int? ParseAsInt(string str)
    {
        int number;

        if (int.TryParse(str, out number))
            return number;
        else
            return null;
    }

    public override string ToString()
    {
        return string.Format("{0}.{1}.{2}", 
            this.Major.HasValue ? this.Major.Value : 0,
            this.Minor.HasValue ? this.Minor.Value : 0,
            this.Revision.HasValue ? this.Revision.Value : 0);
    }

    public int CompareTo(SoftwareVersion? other)
    {
        if ((this.Major ?? 0) > (other.Major ?? 0))
            return 1;
        else if ((this.Major ?? 0) < (other.Major ?? 0))
            return -1;

        if ((this.Minor ?? 0) > (other.Minor ?? 0))
            return 1;
        else if ((this.Minor ?? 0) < (other.Minor ?? 0))
            return -1;

        if ((this.Revision ?? 0) > (other.Revision ?? 0))
            return 1;
        else if ((this.Revision ?? 0) < (other.Revision ?? 0))
            return -1;

        return 0;
    }

    public static bool operator >(SoftwareVersion left, SoftwareVersion right)
    {
        if (left.CompareTo(right) == 1)
            return true;
        else
            return false;
    }

    public static bool operator <(SoftwareVersion left, SoftwareVersion right)
    {
        if (left.CompareTo(right) == -1)
            return true;
        else
            return false;
    }
}
