namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class ApiConfigRepository : IApiConfigRepository
{
    private readonly ISources _sources;
    private readonly Lazy<ApiConfig> _scriptObject;

    public ApiConfigRepository(ISources sources)
    {
        _sources = sources;
        _scriptObject = new Lazy<ApiConfig>(GetLazyConfig);
    }

    public ApiConfig Get()
    {
        return _scriptObject.Value;
    }

    private ApiConfig GetLazyConfig()
    {
        return new ApiConfig
        {
            Namespace = _sources.Compilation.AssemblyName,
        };
    }
}