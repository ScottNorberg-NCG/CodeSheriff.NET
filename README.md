# CodeSheriff.NET

CodeSheriff is a security scanning tool built for websites built with ASP.NET Core. It exists because other scanners on the market do not find enough vulnerabilites and find far too many false positives.

What makes CodeSheriff different from other scanners available today? It uses the .NET Compiler Platform (Roslyn) to analyze code and find vulnerabilities, allowing the scanner to perform a deeper (and more accurate) analysis.

## Using CodeSheriff.NET

To use CodeSheriff, start CodeSheriff.LocalUI for a form to allow you to choose the solution to scan, your output file location, etc.

If you are running into issues with the code, you can download the website I use for testing (https://github.com/ScottNorberg-NCG/SASTTest) and run the CodeSheriff.IntegrationTests console app. 

## Reporting Issues

If the scanner makes any mistakes, either in terms of a false positive or a missed vulnerability, please submit an issue **even if you don't think that it's something a scanner can/could understand**. Because this scanner utilizes Roslyn to do its analysis, there is a great deal of flexibility in how it searches for items.

With that said, if you do want to submit an issue, it would be helpful if I have something to test against. The best way to do this is to add the code that is causing the issue to the testing website listed above (https://github.com/ScottNorberg-NCG/SASTTest)
