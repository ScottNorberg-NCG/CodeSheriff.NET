using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings
{
    internal class CallStack
    {
        public List<SourceLocation> Locations { get; set; } = new List<SourceLocation>();

        public CallStack Clone()
        {
            var newCallStack = new CallStack();

            foreach (var location in this.Locations)
            {
                newCallStack.Locations.Add(location);
            }

            return newCallStack;
        }

        public void AddLocation(ExpressionSyntax syntax)
        { 
            var newLocation = new SourceLocation(syntax);
            this.Locations.Add(newLocation);
        }
    }
}
