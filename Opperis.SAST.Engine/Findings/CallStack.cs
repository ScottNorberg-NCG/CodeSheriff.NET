using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Opperis.SAST.Engine.Findings.SourceLocation;

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

        public void AddLocation(MethodDeclarationSyntax symbol)
        {
            var newLocation = new SourceLocation(symbol);
            this.Locations.Add(newLocation);
        }

        public void AddLocation(SyntaxNode symbol)
        {
            var newLocation = new SourceLocation(symbol);
            this.Locations.Add(newLocation);
        }

        public void AddLocation(ISymbol symbol)
        {
            var newLocation = new SourceLocation(symbol);
            this.Locations.Add(newLocation);
        }
    }
}
