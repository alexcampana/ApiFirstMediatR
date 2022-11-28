namespace ApiFirstMediatR.Generator.Services;

internal sealed class AnalyzerDiagnosticReporter : IDiagnosticReporter
{
    public void ReportDiagnostic(Diagnostic diagnostic)
    {
        // Do nothing, we don't want the analyzer reporting generator diagnostics
    }
}