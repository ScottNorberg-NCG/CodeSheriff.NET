using Microsoft.CodeAnalysis;
using Opperis.SCA.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.SCA;

public class AssemblyVersionInfo
{
    public string UniqueIdentifier { get; private set; }
    public string AssemblyName { get; private set; }
    public string VersionString { get; private set; }
    public SoftwareVersion AssemblyVersion { get; private set; }
    public List<string> ProjectsUsedIn { get; } = new List<string>();

    public AssemblyVersionInfo(AssemblyIdentity id)
    {
        this.UniqueIdentifier = id.GetUniqueIdentifier();
        this.AssemblyName = id.Name;
        this.AssemblyVersion = new SoftwareVersion(id.Version);
        this.VersionString = id.Version.ToString();
    }

    public AssemblyVersionInfo(string name, string version)
    {
        this.UniqueIdentifier = $"{name}|{version}";
        this.AssemblyName = name;
        this.AssemblyVersion = new SoftwareVersion(version);
        this.VersionString = version;
    }
}
