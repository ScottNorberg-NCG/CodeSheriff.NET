using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Trufflehog;

public class FileSystemWrapper
{
    public string file { get; set; }
    public int? line { get; set; }
}
