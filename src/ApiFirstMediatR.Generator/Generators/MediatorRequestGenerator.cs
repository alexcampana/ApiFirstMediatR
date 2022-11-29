namespace ApiFirstMediatR.Generator.Generators;

internal sealed class MediatorRequestGenerator : IApiGenerator
{
    private readonly ISources _sources;
    private readonly IRepository<Controller> _controllerRepository;
    private readonly IApiConfigRepository _apiConfigRepository;

    public MediatorRequestGenerator(ISources sources, IRepository<Controller> controllerRepository, IApiConfigRepository apiConfigRepository)
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
            if (controller.Endpoints != null)
            {
                foreach (var endpoint in controller.Endpoints)
                {
                    var endpointSourceText = ApiTemplate.MediatorRequest.Generate(endpoint, projectConfig);
                    _sources.AddSource($"MediatorRequests_{endpoint.MediatorRequestName}.g.cs", endpointSourceText);
                }
            }
        }
    }
}