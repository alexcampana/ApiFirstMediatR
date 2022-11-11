namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class ApiConfigRepository : IApiConfigRepository
{
    private readonly GeneratorExecutionContext _context;
    private readonly Lazy<ApiConfig> _scriptObject;

    public ApiConfigRepository(GeneratorExecutionContext context)
    {
        _context = context;
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
            Namespace = _context.Compilation.AssemblyName,
        };
    }
}