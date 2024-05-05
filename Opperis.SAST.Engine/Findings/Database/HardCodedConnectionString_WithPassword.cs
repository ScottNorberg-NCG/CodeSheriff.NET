using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Database
{
    internal class HardCodedConnectionStringWithPassword : BaseFinding
    {
        internal override Priority Priority
        {
            get
            {
                if (_priority == null)
                    _priority = Priority.Medium;

                return _priority;
            }
        }

        internal override string FindingText { get { return "A hard-coded connection string was found containing a password"; } }

        internal override string Description { get { return "If an attacker is able to get access to source code, with the hard-coded connection string they may be able to access the database without the help of any other vulnerabilities (such as a SQL injection vulnerability)."; } }
    }
}
