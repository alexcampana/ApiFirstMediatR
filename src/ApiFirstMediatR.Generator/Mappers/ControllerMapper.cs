namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class ControllerMapper : IControllerMapper, IOpenApiDocumentMapper<Controller>
{
    private readonly IEndpointMapper _endpointMapper;
    private readonly IOperationNamingRepository _operationNamingRepository;

    public ControllerMapper(IEndpointMapper endpointMapper, IOperationNamingRepository operationNamingRepository)
    {
        _endpointMapper = endpointMapper;
        _operationNamingRepository = operationNamingRepository;
    }

    public IEnumerable<Controller> Map(OpenApiDocument apiSpec)
    {
        var controllerSpecs = apiSpec
            .Paths
            .GroupBy(p => _operationNamingRepository.GetControllerNameByPath(p.Key))
            .ToList();

        foreach (var controllerSpec in controllerSpecs)
        {
            var paths = controllerSpec.ToOpenApiPaths();
            var endpoints = _endpointMapper.Map(paths);

            yield return new Controller
            {
                Name = $"{controllerSpec.Key}Controller".ToPascalCase(), // TODO: Make this configurable
                Endpoints = endpoints
            };
        }
    }
}