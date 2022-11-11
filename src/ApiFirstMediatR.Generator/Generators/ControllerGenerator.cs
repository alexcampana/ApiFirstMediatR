namespace ApiFirstMediatR.Generator.Generators;

internal sealed class ControllerGenerator : IApiGenerator
{
    private readonly GeneratorExecutionContext _context;
    private readonly IRepository<Controller> _controllerRepository;
    private readonly IApiConfigRepository _apiConfigRepository;

    public ControllerGenerator(GeneratorExecutionContext context, IRepository<Controller> controllerRepository, IApiConfigRepository apiConfigRepository)
    {
        _context = context;
        _controllerRepository = controllerRepository;
        _apiConfigRepository = apiConfigRepository;
    }

    public void Generate()
    {
        var projectConfig = _apiConfigRepository.Get();
        var controllers = _controllerRepository.Get();
        
        foreach (var controller in controllers)
        {
            var controllerSourceText = ApiTemplate.Controller.Generate(controller, projectConfig);
            _context.AddSource($"Controllers_{controller.Name}.g.cs", controllerSourceText);
        }
    }
}