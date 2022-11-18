namespace ApiFirstMediatR.Generator.Interfaces;

internal interface IDiagnosticReporter
{
    void ReportDiagnostic(Diagnostic diagnostic);
}