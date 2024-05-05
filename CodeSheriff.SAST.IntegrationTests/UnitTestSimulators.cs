using CodeSheriff.SAST.Engine.Findings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.IntegrationTests;

internal static class Assert
{
    internal static void AreEqual(object expected, object actual, string message)
    {
        try
        {
            if (expected.Equals(actual))
            {
                Console.WriteLine($"PASSED: {message}");
            }
            else
            {
                WriteError($"FAILED: Expected {expected}, Actual {actual}, {message}");
            }
        }
        catch (Exception ex)
        {
            WriteError($"FAILED: AreEqual for test {message} threw exception {ex.Message}");
        }
    }

    internal static void IsTrue(object expected, string message)
    {
        try
        {
            if (Convert.ToBoolean(expected))
            {
                Console.WriteLine($"PASSED: {message}");
            }
            else
            {
                WriteError($"FAILED: IsTrue Failed, {message}");
            }
        }
        catch (Exception ex)
        {
            WriteError($"FAILED: IsTrue for test {message} threw exception {ex.Message}");
        }
    }

    internal static void AllRootLocationsSet(List<BaseFinding> findings, string message)
    {
        try
        {
            if (findings.Count(f => f.RootLocation == null) == 0)
            {
                Console.WriteLine($"PASSED: No null RootLocation values for {message}");
            }
            else
            {
                WriteError($"FAILED: Null RootLocation values for {message}");
            } 
        }
        catch (Exception ex)
        {
            WriteError($"FAILED: IsTrue for test {message} threw exception {ex.Message}");
        }
    }

    private static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
