using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.SyntaxWalkers;

internal interface ISyntaxWalker
{
    internal bool HasRun { get; }
}
