using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.ErrorHandling;

internal class NuGetProcessingError : BaseError
{
    internal override ErrorCategory Category => ErrorCategory.NuGetProcessingError;

    internal NuGetProcessingError(string assemblyName, Exception ex) 
    {
        var wrapper = new Exception($"An error occurred processing assembly {assemblyName}", ex);
        base.BaseException = wrapper;
    }
}
