﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Resources;

internal class UnvalidatedFilePathForCreate : BaseFinding
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

    public override string FindingText => "File Created - Path From UI";

    public override string Description => "We found a call to File.Create() whose path includes input from an untrusted source. This may allow an attacker to overwrite an existing file. Instead, consider using generated filenames and only expose identifiers to untrusted sources.";
}
