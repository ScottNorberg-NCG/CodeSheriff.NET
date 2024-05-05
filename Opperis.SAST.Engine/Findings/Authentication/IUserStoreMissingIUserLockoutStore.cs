using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Authentication;

internal class IUserStoreMissingIUserLockoutStore : BaseFinding
{
    internal override Priority Priority => Priority.Medium;

    internal override string FindingText => "An object implementing IUserStore was found, but the object did not also implement IUserLockoutStore";

    internal override string Description => "If the IUserStore does not also implement IUserLockoutStore, the lockout mechanisms built into the ASP.NET authentication framework do not function properly (and no errors are thrown).";

    public IUserStoreMissingIUserLockoutStore(ISymbol symbol)
    { 
        this.RootLocation = new SourceLocation(symbol);
    }
}
