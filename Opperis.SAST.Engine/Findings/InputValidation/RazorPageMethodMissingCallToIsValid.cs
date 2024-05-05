using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SAST.Engine.Findings.InputValidation
{
    internal class RazorPageMethodMissingCallToIsValid : BaseFinding
    {
        internal override Priority Priority => Priority.MediumLow;

        internal override string FindingText => "A Razor Page method is missing a call to ModelState.IsValid";

        internal override string Description => "By skipping a call to ModelState.IsValid, any input validation done on model objects may be missed and bad input may be processed by your system.";

        internal RazorPageMethodMissingCallToIsValid(MethodDeclarationSyntax method)
        {
            this.RootLocation = new SourceLocation(method);

            var callStack = new CallStack();
            callStack.AddLocation(method);

            var classDeclaration = method.Parent;
            while (classDeclaration != null)
            {
                if (classDeclaration is ClassDeclarationSyntax)
                {
                    callStack.AddLocation(classDeclaration);
                }

                classDeclaration = classDeclaration.Parent;
            }

            this.CallStacks.Add(callStack);
        }
    }
}
