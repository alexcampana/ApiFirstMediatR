namespace ApiFirstMediatR.Generator.Generators;

internal sealed class MediatorRequestGenerator : IApiGenerator
{
    private readonly GeneratorExecutionContext _context;
    private readonly IRepository<Controller> _controllerRepository;

    public MediatorRequestGenerator(GeneratorExecutionContext context, IRepository<Controller> controllerRepository)
    {
        _context = context;
        _controllerRepository = controllerRepository;
    }

    public void Generate()
    {
        
        var projectConfig = new ScriptObject();
        projectConfig.Add("namespace", _context.Compilation.AssemblyName);

        var controllers = _controllerRepository.Get();

        foreach (var controller in controllers)
        {
            var controllerSourceText = ApiTemplate.Controller.Generate(controller, projectConfig);
            _context.AddSource($"Controllers_{controller.Name}.g.cs", controllerSourceText);
        }
    }
}