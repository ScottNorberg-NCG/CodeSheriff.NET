using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Database;

internal class SqlInjection_DataFromOther : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.High;

            return _priority;
        }
    }

    public override string FindingText { get { return "Possible SQL Injection (Data Sources Not Identified)"; } }

    public override string Description { get { return "A database query was found that includes text that is not hard-coded. If this comes from an untrusted source, this may lead to attackers hijacking the database connection and steal, modify, or delete data."; } }
}
