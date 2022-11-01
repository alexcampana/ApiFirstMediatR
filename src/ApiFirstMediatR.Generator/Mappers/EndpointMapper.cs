namespace ApiFirstMediatR.Generator.Mappers;

internal static class EndpointMapper
{
    public static IEnumerable<Endpoint> Map(OpenApiPaths paths)
    {
        foreach (var path in paths)
        {
            foreach (var endpoint in path.Value.Operations)
            {
                var endpointName = endpoint.Value.OperationId ?? $"{endpoint.Key.GetDisplayName()}{PathToEndpointName(path.Key)}";
                yield return new Endpoint(endpointName, path.Key, endpoint.Key);
            }
        }
    }

    private static string PathToEndpointName(this string path)
    {
        var pathParts  = path
            .Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(ps => ps.Replace("{", "").Replace("}", "").ToPascalCase());

        return string.Join("", pathParts);
    }
}