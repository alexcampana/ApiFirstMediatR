namespace ApiFirstMediatR.Generator.Generators;

internal sealed class ControllerGenerator : IApiGenerator
{
    private readonly GeneratorExecutionContext _context;
    private readonly IRepository<Controller> _controllerRepository;

    public ControllerGenerator(GeneratorExecutionContext context, IRepository<Controller> controllerRepository)
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
            if (controller.Endpoints != null)
            {
                foreach (var endpoint in controller.Endpoints)
                {
                    var endpointSourceText = ApiTemplate.MediatorRequest.Generate(endpoint, projectConfig);
                    _context.AddSource($"MediatorRequests_{endpoint.MediatorRequestName}.g.cs", endpointSourceText);
                }
            }
        }
    }
}