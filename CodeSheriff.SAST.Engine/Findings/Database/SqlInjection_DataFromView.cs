using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Database;

internal class SqlInjection_DataFromView : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.VeryHigh;

            return _priority;
        }
    }

    public override string FindingText { get { return "SQL Injection - Data From View"; } }

    public override string Description { get { return "A database query was found that includes text that appears to come directly from the user via request data. This may lead to attackers hijacking the database connection and steal, modify, or delete data."; } }
}
