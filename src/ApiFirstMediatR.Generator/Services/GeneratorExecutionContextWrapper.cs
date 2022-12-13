namespace ApiFirstMediatR.Generator.Services;

internal sealed class GeneratorExecutionContextWrapper : IDiagnosticReporter, ISources, ICompilation
{
    private readonly GeneratorExecutionContext _context;

    public GeneratorExecutionContextWrapper(GeneratorExecutionContext context)
    {
        _context = context;
    }

    public void ReportDiagnostic(Diagnostic diagnostic)
    {
        _context.ReportDiagnostic(diagnostic);
    }

    public void AddSource(string hintName, string source)
    {
        _context.AddSource(hintName, source);
    }

    public ImmutableArray<AdditionalText> AdditionalFiles => _context.AdditionalFiles;
    public CancellationToken CancellationToken => _context.CancellationToken;
    public Compilation Compilation => _context.Compilation;
    public AnalyzerConfigOptionsProvider AnalyzerConfigOptions => _context.AnalyzerConfigOptions;
}