using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Cryptography;

internal class UseOfDeprecatedSymmetricAlgorithm : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.Medium;

            return _priority;
        }
    }

    public override string FindingText { get { return "Use of Deprecated Symmetric Encryption Algorithm"; } }
    public override string Description { get { return "Use of a deprecated symmetric encryption algorithm was found. AES is the best choice of algorithm in most cases."; } }
}
