using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings.InputValidation
{
    internal class ControllerBinderMissingValidators : BaseFinding
    {
        internal override Priority Priority => Priority.MediumLow;

        internal override string FindingText => "A Controller method parameter is missing validators";

        internal override string Description => "An object was found that is missing validation attributes. This may lead to your application processing bad data, both security-related and not.";

        internal ControllerBinderMissingValidators(MethodDeclarationSyntax method) 
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
