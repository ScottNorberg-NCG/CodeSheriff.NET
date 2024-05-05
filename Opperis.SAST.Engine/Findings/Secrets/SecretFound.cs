using CodeSheriff.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Secrets
{
    internal class SecretFound : BaseFinding
    {
        internal override Priority Priority => Priority.Medium;

        internal override string FindingText => "A hard-coded secret (such as password or API key) was found";

        private string _description;
        internal override string Description => _description;

        public SecretFound(GitLeaksRule rule)
        {
            this._description = rule.description;
        }
    }
}
