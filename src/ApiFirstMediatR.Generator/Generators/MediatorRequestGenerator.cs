namespace ApiFirstMediatR.Generator.Generators;

internal sealed class MediatorRequestGenerator : IApiGenerator
{
    private readonly GeneratorExecutionContext _context;
    private readonly IRepository<Controller> _controllerRepository;
    private readonly IApiConfigRepository _apiConfigRepository;

    public MediatorRequestGenerator(GeneratorExecutionContext context, IRepository<Controller> controllerRepository, IApiConfigRepository apiConfigRepository)
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