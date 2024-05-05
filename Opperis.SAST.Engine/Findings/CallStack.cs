using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CodeSheriff.SAST.Engine.Findings.SourceLocation;

namespace CodeSheriff.SAST.Engine.Findings
{
    internal class CallStack
    {
        private List<SourceLocation> _locations = new List<SourceLocation>();
        public ReadOnlyCollection<SourceLocation> Locations
        {
            get 
            {
                return _locations.AsReadOnly();
            }
        }

        public CallStack Clone()
        {
            var newCallStack = new CallStack();

            foreach (var location in this.Locations)
            {
                newCallStack._locations.Add(location);
            }

            return newCallStack;
        }

        public bool AddLocation(ExpressionSyntax syntax)
        {
            if (_locations.Count >= Globals.MaxCallStackDepth)
                return false;

            var newLocation = new SourceLocation(syntax);
            _locations.Add(newLocation);

            return true;
        }

        public bool AddLocation(MethodDeclarationSyntax symbol)
        {
            if (_locations.Count >= Globals.MaxCallStackDepth)
                return false;

            var newLocation = new SourceLocation(symbol);
            _locations.Add(newLocation);

            return true;
        }

        public bool AddLocation(SyntaxNode symbol)
        {
            if (_locations.Count >= Globals.MaxCallStackDepth)
                return false;

            var newLocation = new SourceLocation(symbol);
            _locations.Add(newLocation);

            return true;
        }

        public bool AddLocation(ISymbol symbol)
        {
            if (_locations.Count >= Globals.MaxCallStackDepth)
                return false;

            var newLocation = new SourceLocation(symbol);
            _locations.Add(newLocation);

            return true;
        }
    }
}
