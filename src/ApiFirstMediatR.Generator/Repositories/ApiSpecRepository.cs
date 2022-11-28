namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class ApiSpecRepository : IApiSpecRepository
{
    private readonly ICompilation _compilation;
    private readonly IDiagnosticReporter _diagnosticReporter;
    private readonly Lazy<OpenApiDocument?> _openApiDocument;

    public ApiSpecRepository(ICompilation compilation, IDiagnosticReporter diagnosticReporter)
    {
        _compilation = compilation;
        _diagnosticReporter = diagnosticReporter;
        _openApiDocument = new Lazy<OpenApiDocument?>(Parse);
    }

    public OpenApiDocument? Get()
    {
        return _openApiDocument.Value;
    }

    private OpenApiDocument? Parse()
    {
        var specFiles = _compilation
            .AdditionalFiles
            .Where(f => f.Path.EndsWith(".yaml", StringComparison.InvariantCultureIgnoreCase) ||
                        f.Path.EndsWith(".yml", StringComparison.InvariantCultureIgnoreCase) ||
                        f.Path.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        if (specFiles.Count == 0)
        {
            _diagnosticReporter.ReportDiagnostic(DiagnosticCatalog.ApiSpecFileNotFound());
            return null;
        }

        foreach (var specFile in specFiles)
        {
            var fileContent = specFile.GetText(_compilation.CancellationToken);

            if (fileContent is null || fileContent.Length == 0)
            {
                var diagnostic = DiagnosticCatalog.ApiSpecFileEmpty(specFile.GetLocation());
                _diagnosticReporter.ReportDiagnostic(diagnostic);
                continue;
            }

            var apiSpec = new OpenApiStringReader().Read(fileContent.ToString(), out var apiDiagnostic);

            if (apiDiagnostic.Errors.Any())
            {
                var diagnostic =
                    DiagnosticCatalog.ApiSpecFileParsingError(specFile.GetLocation(),
                        apiDiagnostic.Errors.First().Message);
                _diagnosticReporter.ReportDiagnostic(diagnostic);
                continue;
            }

            return apiSpec; // Only processing the first valid API Spec file that we find
            // TODO: Throw diagnostic if we find multiple valid API Spec files
        }

        return null;
    }
}