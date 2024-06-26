﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Resources;

internal class UnvalidatedFilePathForMove : BaseFinding
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

    public override string FindingText => "File Moved - Path From UI";

    public override string Description => "We found a call to File.Move() whose source or destination paths includes input from an untrusted source. This may allow an attacker to move or overwrite an arbitrary file. Instead, consider using generated filenames and only expose identifiers to untrusted sources.";
}
