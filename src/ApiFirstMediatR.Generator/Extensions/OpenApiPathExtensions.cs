namespace ApiFirstMediatR.Generator.Extensions;

internal static class OpenApiPathExtensions
{
    public static OpenApiPaths ToOpenApiPaths(this IEnumerable<KeyValuePair<string, OpenApiPathItem>> grouping)
    {
        var paths = new OpenApiPaths();
        
        foreach (var value in grouping)
        {
            paths.Add(value.Key, value.Value);
        }

        return paths;
    }
}