using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Resources
{
    internal class UnvalidatedFilePathForRead : BaseFinding
    {
        internal override Priority Priority
        {
            get
            {
                if (_priority == null)
                    _priority = Priority.High;

                return _priority;
            }
        }

        internal override string FindingText => "File Read - Path From UI";

        internal override string Description => "We found a call to File.Read() or File.Open() whose path includes input from an untrusted source. This may allow an attacker to read an arbitrary file. Instead, consider using generated filenames and only expose identifiers to untrusted sources.";
    }
}
