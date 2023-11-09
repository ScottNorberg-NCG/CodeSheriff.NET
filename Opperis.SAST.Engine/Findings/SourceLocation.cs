using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Operations;
using Opperis.SAST.Engine.RoslynObjectExtensions;

namespace Opperis.SAST.Engine.Findings
{
    internal class SourceLocation
    {
        public enum SyntaxType
        {
            ClassConstructor,
            ClassDeclaration,
            ClassProperty,

            CshtmlFile,

            InterfaceDeclaration,

            InterpolatedString,

            MethodArgument,
            MethodCall,
            MethodDeclaration,
            MethodParameter,

            NamedItem,

            StructDeclaration,

            VariableAssignment,
            VariableCreation,
        }

        public string Text { get; set; }
        public int LineNumber { get; set; }
        public string FilePath { get; set; }
        public SyntaxType? LocationType { get; set; }
        public object? Symbol { get; set; }

        public SourceLocation() { }

        public SourceLocation(ExpressionSyntax symbol)
        {
            var lineSpan = symbol.SyntaxTree.GetLineSpan(symbol.Span);

            //this.Text = symbol.GetDisplayText();
            this.LineNumber = lineSpan.StartLinePosition.Line + 1;
            this.FilePath = symbol.SyntaxTree.FilePath;
            //this.LocationType = symbol.GetType().Name;
            this.Symbol = symbol;

            if (symbol is MemberAccessExpressionSyntax memberAccess)
            {
                this.Text = memberAccess.Name.Identifier.Text;
                this.LocationType = SyntaxType.MethodCall; //Is this always true?
            }
            else if (symbol is IdentifierNameSyntax id)
            {
                this.Text = id.Identifier.Text;
                this.LocationType = SyntaxType.NamedItem;
            }
            else if (symbol is InterpolatedStringExpressionSyntax interpolated)
            {
                this.Text = interpolated.GetText().ToString();
                this.LocationType = SyntaxType.InterpolatedString;
            }
            else if (symbol is ArrayCreationExpressionSyntax array)
            {
                this.Text = array.GetText().ToString();
                this.LocationType = SyntaxType.InterpolatedString;
            }
            else if (symbol is AssignmentExpressionSyntax assignment)
            {
                this.Text = assignment.ToString();
                this.LocationType = SyntaxType.VariableAssignment;
            }
            else if (symbol is InvocationExpressionSyntax invocation)
            {
                this.Text = invocation.ToString();
                this.LocationType = SyntaxType.MethodCall;
            }
            else if (symbol is BinaryExpressionSyntax binary)
            {
                this.Text = binary.ToString();
                this.LocationType = SyntaxType.InterpolatedString; //TODO: Is this always true?
            }
            else if (symbol is ObjectCreationExpressionSyntax constructor)
            { 
                this.Text = constructor.ToString();
                this.LocationType = SyntaxType.ClassConstructor;
            }
            else
                throw new NotImplementedException();
        }

        public SourceLocation(MethodDeclarationSyntax symbol)
        {
            var lineSpan = symbol.SyntaxTree.GetLineSpan(symbol.Span);

            this.Text = symbol.Identifier.Text;
            this.LineNumber = lineSpan.StartLinePosition.Line + 1;
            this.FilePath = symbol.SyntaxTree.FilePath;
            this.LocationType = SyntaxType.MethodDeclaration;
            this.Symbol = symbol;
        }

        public SourceLocation(SyntaxNode symbol)
        {
            var lineSpan = symbol.SyntaxTree.GetLineSpan(symbol.Span);
            this.LineNumber = lineSpan.StartLinePosition.Line + 1;
            this.FilePath = symbol.SyntaxTree.FilePath;
            this.Symbol = symbol;

            if (symbol is VariableDeclaratorSyntax)
            {
                this.Text = symbol.ToString();
                this.LocationType = SyntaxType.VariableCreation;
            }
            else if (symbol is ClassDeclarationSyntax classDeclaration)
            {
                this.Text = classDeclaration.Identifier.Text;
                this.LocationType = SyntaxType.ClassDeclaration;
            }
            else if (symbol is ParameterSyntax)
            {
                this.Text = symbol.ToString();
                this.LocationType = SyntaxType.MethodParameter;
            }
            else if (symbol is ArgumentSyntax)
            {
                this.Text = symbol.ToString();
                this.LocationType = SyntaxType.MethodArgument;
            }
            else if (symbol is InterfaceDeclarationSyntax interfaceSyntax)
            {
                this.Text = interfaceSyntax.Identifier.Text;
                this.LocationType = SyntaxType.InterfaceDeclaration;
            }
            else if (symbol is StructDeclarationSyntax structSyntax)
            {
                this.Text = structSyntax.Identifier.Text;
                this.LocationType = SyntaxType.StructDeclaration;
            }
            else
                throw new NotImplementedException();
        }

        public SourceLocation(ISymbol symbol)
        {
            var location = symbol.Locations.First();

            if (location.SourceTree != null)
            {
                var syntaxTree = location.SourceTree;
                var syntaxTreeRoot = syntaxTree.GetRoot();
                var syntaxNode = syntaxTreeRoot.FindNode(location.SourceSpan);

                var lineSpan = syntaxTree.GetLineSpan(syntaxNode.Span);

                this.Text = syntaxNode.GetDisplayText();
                this.LineNumber = lineSpan.StartLinePosition.Line + 1;
                this.FilePath = syntaxTree.FilePath;
            }
            else
            {
                this.Text = symbol.ToString();
                this.FilePath = "(External)";
            }

            if (symbol is IMethodSymbol)
            {
                this.LocationType = SyntaxType.MethodDeclaration;
            }
            else if (symbol is INamedTypeSymbol)
            {
                this.LocationType = SyntaxType.NamedItem;
            }
            else if (symbol is ILocalSymbol)
            {
                //TODO: Verify
                this.LocationType = SyntaxType.VariableCreation;
            }
            else if (symbol is IPropertySymbol)
            {
                this.LocationType = SyntaxType.ClassProperty;
            }
            else
            {
                throw new NotImplementedException();
            }

            this.Symbol = symbol;
            //this.LocationType = symbol.GetType().Name;
        }

        public override string ToString()
        {
            return $"{Text}, Line: {LineNumber}, File: {FilePath}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (!(obj is SourceLocation))
                return false;

            var other = obj as SourceLocation;

            return other.LineNumber == this.LineNumber && other.FilePath == this.FilePath && other.Text == this.Text;
        }
    }
}
