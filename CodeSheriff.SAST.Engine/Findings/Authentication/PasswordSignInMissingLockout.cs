using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Authentication;

internal class PasswordSignInMissingLockout : BaseFinding
{
    public override Priority Priority => Priority.Medium;

    public override string FindingText => "Login found with Lockout on Failure set to false";

    public override string Description => "Attackers may try to guess passwords in a brute force attack. Setting lockoutOnFailure = true will reduce the likelihood that an attacker will get in.";
}
