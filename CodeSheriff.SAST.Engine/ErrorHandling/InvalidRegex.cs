using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.ErrorHandling;

internal class InvalidRegex : BaseError
{
    public override ErrorCategory Category => ErrorCategory.InvalidRegex;

    public InvalidRegex(string regex, Exception ex)
    {
        var wrapper = new ApplicationException($"Regex {regex} could not be processed", ex);
        base.BaseException = wrapper;
    }
}
