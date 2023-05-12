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
            !string.IsNullOrEmpty(serializationLibraryName))
        {
            if (!SerializationLibrary.TryGetSerializationLibrary(serializationLibraryName, out serializationLibrary))
            {
                _diagnosticReporter.ReportDiagnostic(DiagnosticCatalog.InvalidSerializationLibrary(Location.None, serializationLibraryName));
            }
        }

        OperationGenerationMode? operationGenerationMode = null;
        if (_compilation.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ApiFirstMediatR_OperationGenerationMode", out var operationGenerationModeName) &&
            !string.IsNullOrEmpty(operationGenerationModeName))
        {
            if (!Enum.TryParse<OperationGenerationMode>(operationGenerationModeName, out var operationGenerationModeEnum))
            {
                _diagnosticReporter.ReportDiagnostic(
                    DiagnosticCatalog.InvalidOperationGenerationMode(Location.None, operationGenerationModeName));
            }
            else
            {
                operationGenerationMode = operationGenerationModeEnum;
            }
        }

        _compilation.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ApiFirstMediatR_RequestBodyName", out var requestBodyName);

        return new ApiConfig
        {
            Namespace = _compilation.Compilation.AssemblyName ?? "ApiFirst",
            SerializationLibrary = serializationLibrary ?? SerializationLibrary.SystemTextJson,
            RequestBodyName = requestBodyName ?? "Body",
            OperationGenerationMode = operationGenerationMode ?? OperationGenerationMode.MultipleClientsFromPathSegmentAndOperationId
        };
    }
}