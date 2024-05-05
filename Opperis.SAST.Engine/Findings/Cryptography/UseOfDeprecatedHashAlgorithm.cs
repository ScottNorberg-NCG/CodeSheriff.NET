using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Cryptography
{
    internal class UseOfDeprecatedHashAlgorithm : BaseFinding
    {
        internal override Priority Priority
        {
            get
            {
                if (_priority == null)
                    _priority = Priority.MediumLow;

                return _priority;
            }
        }

        internal override string FindingText { get { return "Use of Deprecated Hashing Algorithm"; } }
        internal override string Description { get { return "Use of a deprecated hashing algorithm was found. Algorithms in the SHA3 family are the best choice of algorithm in most cases."; } }
    }
}
