using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeSheriff.SAST.Engine.Trufflehog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.Secrets
{
    internal class TrufflehogSecret : BaseFinding
    {
        private Priority _priority;
        internal override Priority Priority => _priority;

        private string _findingText;
        internal override string FindingText => _findingText;

        private string _description;
        internal override string Description => _description;

        public TrufflehogSecret(Result result)
        {
            if (result.Verified)
            {
                _priority = Priority.Medium;
                _findingText = "A verified secret (such as a hard-coded API key) was found by Trufflehog";
            }
            else
            {
                _priority = Priority.Info;
                _findingText = "A secret (such as a hard-coded API key) was found by Trufflehog that could not be verified.";
            }

            _description = $"A secret of type {result.DetectorName} was found in source code. If source code is accidentally left public or is stolen, this will lead to the API key being leaked to potential criminals";

            this.RootLocation = new SourceLocation();
            this.RootLocation.Text = result.Redacted;
            this.RootLocation.FilePath = result.SourceMetadata.Data.Filesystem.file;
            this.RootLocation.LineNumber = result.SourceMetadata.Data.Filesystem.line ?? -1;
        }
    }
}
