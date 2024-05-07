# CodeSheriff.NET

CodeSheriff is a security scanning tool built for websites built with ASP.NET Core. It exists because other scanners on the market do not find enough vulnerabilites and find far too many false positives.

What makes CodeSheriff different from other scanners available today? It uses the .NET Compiler Platform (Roslyn) to analyze code and find vulnerabilities, allowing the scanner to perform a deeper (and more accurate) analysis.

## Using CodeSheriff.NET

To use CodeSheriff, start CodeSheriff.LocalUI for a form to allow you to choose the solution to scan, your output file location, etc.

If you are running into issues with the code, you can download the website I use for testing (https://github.com/ScottNorberg-NCG/SASTTest) and run the CodeSheriff.IntegrationTests console app. 
