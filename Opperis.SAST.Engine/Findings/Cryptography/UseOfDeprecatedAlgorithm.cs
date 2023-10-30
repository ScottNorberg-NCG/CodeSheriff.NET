using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.Cryptography
{
    internal class UseOfDeprecatedAlgorithm : BaseFinding
    {
        internal override string Priority_Text { get { return "Medium"; } }
        internal override float Priority_Sort { get { return 3f; } }
        internal override string FindingText { get { return "Use of Deprecated Symmetric Encryption Algorithm"; } }
        internal override string Description { get { return "Use of a deprecated symmetric encryption algorithm was found. AES is the best choice of algorithm in most cases."; } }
    }
}
