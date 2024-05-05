using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Cryptography
{
    internal class HardCodedCryptographyIV : BaseFinding
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

        internal override string FindingText { get { return "Use of Hard-Coded Cryptography IV"; } }

        internal override string Description { get { return "A hard-coded IV was found for a symmetric key algorithm. These should be generated via a secure random number generator and not hard-coded. The IVs can be stored along with your cipher texts if you keep the keys safe."; } }
    }
}
