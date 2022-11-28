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
        return new ApiConfig
        {
            Namespace = _compilation.Compilation.AssemblyName,
        };
    }
}