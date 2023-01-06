namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class ApiConfigRepository : IApiConfigRepository
{
    private readonly ICompilation _compilation;
    private readonly IDiagnosticReporter _diagnosticReporter;
    private readonly Lazy<ApiConfig> _scriptObject;

    public ApiConfigRepository(ICompilation compilation, IDiagnosticReporter diagnosticReporter)
    {
        _compilation = compilation;
        _diagnosticReporter = diagnosticReporter;
        _scriptObject = new Lazy<ApiConfig>(GetLazyConfig);
    }

    public ApiConfig Get()
    {
        return _scriptObject.Value;
    }

    private ApiConfig GetLazyConfig()
    {
        SerializationLibrary? serializationLibrary = null;
        
        if (_compilation.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ApiFirstMediatR_SerializationLibrary", out var serializationLibraryName) &&
            !String.IsNullOrEmpty(serializationLibraryName))
        {
            if (!SerializationLibrary.TryGetSerializationLibrary(serializationLibraryName, out serializationLibrary))
            {
                _diagnosticReporter.ReportDiagnostic(DiagnosticCatalog.InvalidSerializationLibrary(Location.None, serializationLibraryName));
            }
        }
        
        _compilation.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ApiFirstMediatR_RequestBodyName", out var requestBodyName);

        return new ApiConfig
        {
            Namespace = _compilation.Compilation.AssemblyName ?? "ApiFirst",
            SerializationLibrary = serializationLibrary ?? SerializationLibrary.SystemTextJson,
            RequestBodyName = requestBodyName ?? "Body"
        };
    }
}