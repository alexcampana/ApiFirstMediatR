namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class ApiConfigRepository : IApiConfigRepository
{
    private readonly GeneratorExecutionContext _context;
    private readonly Lazy<ScriptObject> _scriptObject;

    public ApiConfigRepository(GeneratorExecutionContext context)
    {
        _context = context;
        _scriptObject = new Lazy<ScriptObject>(GetLazyConfig);
    }

    public ScriptObject Get()
    {
        return _scriptObject.Value;
    }

    private ScriptObject GetLazyConfig()
    {
        var projectConfig = new ScriptObject();
        projectConfig.Add("namespace", _context.Compilation.AssemblyName);

        return projectConfig;
    }
}