using Microsoft.OpenApi.Any;

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

    public IEnumerable<Controller> Map(OpenApiDocument[] apiSpecs)
    {
        foreach (var apiSpec in apiSpecs)
        {
            var ns = apiSpec.Extensions.TryGetValue("x-namespace", out var documentNamespace) ? ((OpenApiString)documentNamespace).Value : "default";

            var controllerSpecs = apiSpec
                .Paths
                .GroupBy(p => _operationNamingRepository.GetControllerNameByPath(p.Key))
                .ToList();

            foreach (var controllerSpec in controllerSpecs)
            {
                var paths = controllerSpec.ToOpenApiPaths();
                var endpoints = _endpointMapper.Map(paths, ns);

                yield return new Controller
                {
                    Name = $"{controllerSpec.Key}Controller".ToPascalCase(), // TODO: Make this configurable
                    Endpoints = endpoints,
                    Namespace = ns,
                };
            }
        }
    }
}