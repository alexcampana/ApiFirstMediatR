namespace ApiFirstMediatR.Generator.Generators;

internal sealed class ControllerGenerator : IApiGenerator
{
    private readonly ISources _sources;
    private readonly IRepository<Controller> _controllerRepository;
    private readonly IApiConfigRepository _apiConfigRepository;

    public ControllerGenerator(ISources sources, IRepository<Controller> controllerRepository, IApiConfigRepository apiConfigRepository)
    {
        _sources = sources;
        _controllerRepository = controllerRepository;
        _apiConfigRepository = apiConfigRepository;
    }

    public void Generate()
    {
        var projectConfig = _apiConfigRepository.Get();
        var controllers = _controllerRepository.Get();
        
        foreach (var controller in controllers)
        {
            var baseNamespace = projectConfig.Namespace;
            if (!string.IsNullOrWhiteSpace(controller.Namespace) && controller.Namespace != "default")
            {
                projectConfig.Namespace = controller.Namespace;
            }
            var controllerSourceText = ApiTemplate.Controller.Generate(controller, projectConfig);
            _sources.AddSource($"{controller.Namespace}/Controllers_{controller.Name}.g.cs", controllerSourceText);
            projectConfig.Namespace = baseNamespace;
        }
    }
}