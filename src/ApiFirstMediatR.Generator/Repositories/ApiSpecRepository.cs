namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class ApiSpecRepository : IApiSpecRepository
{
    private readonly GeneratorExecutionContext _context;
    private readonly Lazy<OpenApiDocument?> _openApiDocument;

    public ApiSpecRepository(GeneratorExecutionContext context)
    {
        _context = context;
        _openApiDocument = new Lazy<OpenApiDocument?>(Parse);
    }

    public OpenApiDocument? Get()
    {
        return _openApiDocument.Value;
    }

    private OpenApiDocument? Parse()
    {
        var specFiles = _context
            .AdditionalFiles
            .Where(f => f.Path.EndsWith(".yaml", StringComparison.InvariantCultureIgnoreCase) ||
                        f.Path.EndsWith(".yml", StringComparison.InvariantCultureIgnoreCase) ||
                        f.Path.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        if (specFiles.Count == 0)
        {
            _context.ReportDiagnostic(DiagnosticCatalog.ApiSpecFileNotFound());
            return null;
        }

        foreach (var specFile in specFiles)
        {
            var fileContent = specFile.GetText(_context.CancellationToken);

            if (fileContent is null || fileContent.Length == 0)
            {
                var diagnostic = DiagnosticCatalog.ApiSpecFileEmpty(specFile.GetLocation());
                _context.ReportDiagnostic(diagnostic);
                continue;
            }

            var apiSpec = new OpenApiStringReader().Read(fileContent.ToString(), out var apiDiagnostic);

            if (apiDiagnostic.Errors.Any())
            {
                var diagnostic =
                    DiagnosticCatalog.ApiSpecFileParsingError(specFile.GetLocation(),
                        apiDiagnostic.Errors.First().Message);
                _context.ReportDiagnostic(diagnostic);
                continue;
            }

            return apiSpec; // Only processing the first valid API Spec file that we find
            // TODO: Throw diagnostic if we find multiple valid API Spec files
        }

        return null;
    }
}