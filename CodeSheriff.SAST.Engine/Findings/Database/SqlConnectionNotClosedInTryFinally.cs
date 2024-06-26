﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Database;

internal class SqlConnectionNotClosedInTryFinally : BaseFinding
{
    public override Priority Priority
    {
        get
        {
            if (_priority == null)
                _priority = Priority.Low;

            return _priority;
        }
    }

    public override string FindingText { get { return "SQL Connection Not Safely Closed"; } }

    public override string Description { get { return "A call to Open() was found on a database connection but the subsequent call to Close() was found outside a finally or using block. This may lead to connections staying open indefinitely, which can lead to all connections being used, effectively bringing down your website."; } }
}
