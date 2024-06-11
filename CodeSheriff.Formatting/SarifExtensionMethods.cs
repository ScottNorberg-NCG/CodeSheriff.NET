using CodeSheriff.SAST.Engine.Findings;
using Microsoft.CodeAnalysis.Sarif;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.Formatting;

internal static class SarifExtensionMethods
{
    internal static Location ToSarifLocation(this SourceLocation location)
    {
        Uri? sourceFile;
        if (Uri.TryCreate(location.FilePath, new UriCreationOptions(), out sourceFile))
        {
            return new Location
            {
                PhysicalLocation = new PhysicalLocation
                {
                    ArtifactLocation = new ArtifactLocation
                    {
                        Uri = new Uri(location.FilePath)
                    },
                    Region = new Region
                    {
                        StartLine = location.LineNumber,
                        EndLine = location.LineNumber
                    }
                },
                Message = new Message() { Text = location.Text }
            };
        }
        else
        {
            return new Location
            {
                LogicalLocation = new LogicalLocation
                {
                    Name = location.FilePath
                },
                Message = new Message() { Text = location.Text }
            };
        }
    }
}
