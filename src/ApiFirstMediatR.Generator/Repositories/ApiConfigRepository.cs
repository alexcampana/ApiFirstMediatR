namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class ApiConfigRepository : IApiConfigRepository
{
    private readonly ICompilation _compilation;
    private readonly Lazy<ApiConfig> _scriptObject;

    public ApiConfigRepository(ICompilation compilation)
    {
        _compilation = compilation;
        _scriptObject = new Lazy<ApiConfig>(GetLazyConfig);
    }

    public ApiConfig Get()
    {
        return _scriptObject.Value;
    }

    private ApiConfig GetLazyConfig()
    {
        SerializationLibrary? serializationLibrary = null;
        
        if (_compilation.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ApiFirstMediatR_SerializationLibrary", out var serializationLibraryName))
        {
            SerializationLibrary.TryGetSerializationLibrary(serializationLibraryName, out serializationLibrary);
            // TODO: Check to see if serialization library was found and throw diagnostic if it wasn't
            // TODO: Check for setting on additional file or remove the option to do so
        }

        return new ApiConfig
        {
            Namespace = _compilation.Compilation.AssemblyName ?? "ApiFirst",
            SerializationLibrary = serializationLibrary ?? SerializationLibrary.SystemTextJson
        };
    }
}