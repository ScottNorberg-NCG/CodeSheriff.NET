using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.Cryptography
{
    internal class HardCodedCryptographyKey : BaseFinding
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

        internal override string FindingText { get { return "Use of Hard-Coded Cryptography Key"; } }

        internal override string Description { get { return "A hard-coded key was found for a symmetric key algorithm. These should be generated via a secure random number generator and not hard-coded. The keys should be stored in a secure location (that is not in a config file or source control)."; } }
    }
}
