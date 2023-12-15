using Opperis.SAST.Engine.Trufflehog;
using System.Diagnostics;

namespace Opperis.SAST.Engine;

public class TrufflehogLoader
{
    private List<string> truffleHogRaw = new List<string>();

    public List<Result> LoadForFile(string filePath)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "trufflehog";
            process.StartInfo.ArgumentList.Add("filesystem");
            process.StartInfo.ArgumentList.Add(filePath);
            process.StartInfo.ArgumentList.Add("--json");
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;

            process.OutputDataReceived += Process_OutputDataReceived;
            //process.ErrorDataReceived += Process_ErrorDataReceived;

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            //System.IO.File.AppendAllLines("C:\\temp\\Trufflehog\\Pre_" + Guid.NewGuid() + ".txt", new[] { process.StandardError.ReadToEnd().Replace("\\", "\\\\") });

            process.WaitForExit();

            var results = new List<Result>();

            foreach (var output in truffleHogRaw)
            {
                results.Add(System.Text.Json.JsonSerializer.Deserialize<Result>(output));
            }

            return results;
        }
    }

    //private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    //{
    //    throw new NotImplementedException();
    //}

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
            truffleHogRaw.Add(e.Data);
    }
}
