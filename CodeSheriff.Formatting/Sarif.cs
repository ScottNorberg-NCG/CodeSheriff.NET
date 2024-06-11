using CodeSheriff.SAST.Engine.Findings;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Writers;
using Newtonsoft.Json;
using System.IO;

namespace CodeSheriff.Formatting;

public static class Sarif
{
    public static string Generate(List<BaseFinding> findings)
    {
        var log = new SarifLog();

        log.Version = SarifVersion.Current;

        var run = new Run();

        run.Tool = new Tool
        {
            Driver = new ToolComponent
            {
                Name = "CodeSheriff.NET",
                Version = "1.0.0",
                Organization = "Opperis Technologies LLC"
            }
        };

        run.Results = new List<Result>();

        foreach (var finding in findings) 
        {
            var newResult = new Result();
            newResult.RuleId = finding.GetType().Name;

            //This is awkward, but I don't have a better way of doing this at the moment
            if (finding.Priority.Sort <= 2)
                newResult.Level = FailureLevel.Error;
            else if (finding.Priority.Sort <= 4)
                newResult.Level = FailureLevel.Warning;
            else
                newResult.Level = FailureLevel.Note;

            newResult.Message = new Message { Text = finding.FindingText, Markdown = $"### {finding.Description}" };

            if (finding.RootLocation != null)
            {
                newResult.Locations = new List<Location>();

                newResult.Locations.Add(finding.RootLocation.ToSarifLocation());

                newResult.Stacks = new List<Stack>();

                foreach (var callStack in finding.CallStacks)
                {
                    var cs = new Stack();
                    cs.Frames = new List<StackFrame>();

                    foreach (var location in callStack.Locations)
                    {
                        var frame = new StackFrame();
                        frame.Location = location.ToSarifLocation();
                        frame.Location.Message = new Message() { Text = location.Text };
                        cs.Frames.Add(frame);
                    }

                    newResult.Stacks.Add(cs);
                }
            }

            run.Results.Add(newResult);
        }

        log.Runs = [run];

        var asJson = System.Text.Json.JsonSerializer.Serialize(log);
        return asJson;
    }
}
