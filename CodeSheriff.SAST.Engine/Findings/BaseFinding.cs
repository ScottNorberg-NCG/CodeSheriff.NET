using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.DataCleaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings;

public abstract class BaseFinding
{
    protected Priority? _priority;
    public abstract Priority Priority { get; }
    public abstract string FindingText { get; }
    public abstract string Description { get; }
    public string AdditionalInformation { get; set; } = "(None)";

    public List<CallStack> CallStacks { get; } = new List<CallStack>();
    public SourceLocation RootLocation { get; set; }

    public void RedactAllByteArrays()
    {
        RootLocation.Text = StringRedactor.RedactByteArray(RootLocation.Text);

        foreach (var callStack in CallStacks) 
        {
            foreach (var location in callStack.Locations)
            { 
                location.Text = StringRedactor.RedactByteArray(location.Text);
            }
        }
    }
}
