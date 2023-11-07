using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.Cryptography
{
    internal class UseOfECBMode : BaseFinding
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

        internal override string FindingText { get { return "Use of ECB Mode"; } }

        internal override string Description { get { return "A symmetric encryption algorithm that uses ECB mode was detected. This is the least safe of all modes because repeated text can lead to repeated ciphertext, potentially leaking information to attackers."; } }
    }
}
