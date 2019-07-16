namespace Gu.Roslyn.Asserts.Analyzers
{
    using Microsoft.CodeAnalysis;

    public static class Descriptors
    {
        public static readonly DiagnosticDescriptor NameOfLocalShouldMatchParameter = new DiagnosticDescriptor(
            id: "GURA01",
            title: "Name of local should match parameter.",
            messageFormat: "Name of '{0}' should be '{1}'.",
            category: AnalyzerCategory.Ocd,
            defaultSeverity: DiagnosticSeverity.Hidden,
            isEnabledByDefault: true,
            description: "Name of local should match parameter for max consistency.");

        public static readonly DiagnosticDescriptor IndicateErrorPosition = new DiagnosticDescriptor(
            id: "GURA02",
            title: "Indicate error position.",
            messageFormat: "{0}",
            category: AnalyzerCategory.Correctness,
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Indicate error position with ↓ (alt + 25).");

        public static readonly DiagnosticDescriptor NameToFirstClass = new DiagnosticDescriptor(
            id: "GURA03",
            title: "Name of field should be first class.",
            messageFormat: "Name of '{0}' should be '{1}'.",
            category: AnalyzerCategory.Ocd,
            defaultSeverity: DiagnosticSeverity.Hidden,
            isEnabledByDefault: true,
            description: "Name of field should be first class.");

        public static readonly DiagnosticDescriptor NameClassToMatchAsserts = new DiagnosticDescriptor(
            id: "GURA04",
            title: "Name of class should match asserts.",
            messageFormat: "Name of '{0}' should be '{1}'.",
            category: AnalyzerCategory.Ocd,
            defaultSeverity: DiagnosticSeverity.Hidden,
            isEnabledByDefault: true,
            description: "Name of class should match asserts.");

        public static readonly DiagnosticDescriptor NameFileToMatchClass = new DiagnosticDescriptor(
            id: "GURA05",
            title: "Name file to match class.",
            messageFormat: "Name of '{0}' should be '{1}'.",
            category: AnalyzerCategory.Ocd,
            defaultSeverity: DiagnosticSeverity.Hidden,
            isEnabledByDefault: true,
            description: "Name file to match class.");

        public static readonly DiagnosticDescriptor TestShouldBeInCorrectClass = new DiagnosticDescriptor(
            id: "GURA06",
            title: "Move test to correct class.",
            messageFormat: "Move to '{0}'.",
            category: AnalyzerCategory.Ocd,
            defaultSeverity: DiagnosticSeverity.Hidden,
            isEnabledByDefault: true,
            description: "Move test to correct class.");

        public static readonly DiagnosticDescriptor TestClassShouldBePublicStatic = new DiagnosticDescriptor(
            id: "GURA07",
            title: "Test class should be public static.",
            messageFormat: "'{0}' should be public static.",
            category: AnalyzerCategory.Ocd,
            defaultSeverity: DiagnosticSeverity.Hidden,
            isEnabledByDefault: true,
            description: "Test class should be public static.");

        public static readonly DiagnosticDescriptor ShouldBePublic = new DiagnosticDescriptor(
            id: "GURA08",
            title: "Should be public static.",
            messageFormat: "'{0}' should be public.",
            category: AnalyzerCategory.Ocd,
            defaultSeverity: DiagnosticSeverity.Hidden,
            isEnabledByDefault: true,
            description: "Should be public static.");

        public static readonly DiagnosticDescriptor UseStandardNames = new DiagnosticDescriptor(
            id: "GURA09",
            title: "Use standard names in test code.",
            messageFormat: "Use standard name {0} instead of {1}.",
            category: AnalyzerCategory.Ocd,
            defaultSeverity: DiagnosticSeverity.Hidden,
            isEnabledByDefault: true,
            description: "Use standard names for things that do not have particular meaning in test code.");
    }
}
