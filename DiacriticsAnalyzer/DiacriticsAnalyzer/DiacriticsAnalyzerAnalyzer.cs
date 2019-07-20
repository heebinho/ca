using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DiacriticsAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiacriticsAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DiacriticsAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(c => {
                var identifierToken = c.Node.DescendantTokens().FirstOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken));
                if (identifierToken == null) return;
                if (!identifierToken.Text.ContainsGermanDiacritics()) return;

                var diag = Diagnostic.Create(Rule, identifierToken.GetLocation(), identifierToken.Text);
                c.ReportDiagnostic(diag);
            }, 
                SyntaxKind.ClassDeclaration,
                SyntaxKind.InterfaceDeclaration,
                SyntaxKind.FieldDeclaration,
                SyntaxKind.VariableDeclaration,
                SyntaxKind.MethodDeclaration,
                SyntaxKind.PropertyDeclaration,
                SyntaxKind.Parameter);
        }


    }
}
