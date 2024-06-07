using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.ProgramFlow;

internal class OverpostingViaBindObject : BaseFinding
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

    public override string FindingText { get { return "Entity Framework Object Used as Razor Page [BindProperty]"; } }

    public override string Description { get { return "We found an Entity Framework object that was used as a binding object in a razor page. This can lead to Overposting (or Mass Assignment) attacks where an attacker sends more data than the developer intends but the data is processed due to automatic data binding."; } }
}
