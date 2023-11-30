using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Secrets
{
    public class GitLeaksRule
    {
        public string id { get; set; }
        public string description { get; set; }
        public string regex { get; set; }
    }
}
