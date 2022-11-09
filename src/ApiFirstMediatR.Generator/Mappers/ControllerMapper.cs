namespace ApiFirstMediatR.Generator.Mappers;

internal static class ControllerMapper
{
    public static IEnumerable<Controller> Map(OpenApiDocument apiSpec)
    {
        var controllerSpecs = apiSpec
            .Paths
            .GroupBy(p => p.Key.GetControllerName())
            .ToList();

        foreach (var controllerSpec in controllerSpecs)
        {
            var paths = GetOpenApiPaths(controllerSpec);
            var endpoints = EndpointMapper.Map(paths);

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

    private static string GetControllerName(this string path)
    {
        // TODO: Make this configurable
        return path
            .Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault() ?? "Default";
    }
}