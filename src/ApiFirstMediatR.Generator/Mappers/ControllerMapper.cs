namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class ControllerMapper : IControllerMapper, IOpenApiDocumentMapper<Controller>
{
    private readonly IEndpointMapper _endpointMapper;

    public ControllerMapper(IEndpointMapper endpointMapper)
    {
        _endpointMapper = endpointMapper;
    }

    public IEnumerable<Controller> Map(OpenApiDocument apiSpec)
    {
        var controllerSpecs = apiSpec
            .Paths
            .GroupBy(p => GetControllerName(p.Key))
            .ToList();

        foreach (var controllerSpec in controllerSpecs)
        {
            var paths = GetOpenApiPaths(controllerSpec);
            var endpoints = _endpointMapper.Map(paths);

            yield return new Controller
            {
                Name = $"{controllerSpec.Key}Controller".ToPascalCase(),
                Endpoints = endpoints
            };
        }
    }

    private static OpenApiPaths GetOpenApiPaths(IEnumerable<KeyValuePair<string, OpenApiPathItem>> grouping)
    {
        var paths = new OpenApiPaths();
        
        foreach (var value in grouping)
        {
            paths.Add(value.Key, value.Value);
        }

        return paths;
    }

    private static string GetControllerName(string path)
    {
        // TODO: Make this configurable
        return path
            .Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault() ?? "Default";
    }
}