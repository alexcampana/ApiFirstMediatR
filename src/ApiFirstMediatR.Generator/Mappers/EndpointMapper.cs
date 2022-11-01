namespace ApiFirstMediatR.Generator.Mappers;

internal static class EndpointMapper
{
    public static IEnumerable<Endpoint> Map(OpenApiPaths paths)
    {
        foreach (var path in paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                var endpointName = operation.Value.OperationId ?? $"{operation.Key.GetDisplayName()}{PathToEndpointName(path.Key)}";

                var queryParams = ParameterMapper.Map(operation.Value.Parameters.Where(p => p.In == ParameterLocation.Query));
                var pathParams = ParameterMapper.Map(operation.Value.Parameters.Where(p => p.In == ParameterLocation.Path));
                // TODO: Add support for Cookie, Header params
                
                var endpoint = new Endpoint
                {
                    Name = endpointName.ToPascalCase(),
                    Path = path.Key,
                    OperationName = operation.Key.GetDisplayName().ToPascalCase(),
                    QueryParameters = queryParams,
                    PathParameters = pathParams
                };

                if (operation.Value.RequestBody is not null &&
                    operation.Value.RequestBody.Content.TryGetValue("application/json", out var requestBody))
                {
                    endpoint.RequestBodyType = TypeMapper.Map(requestBody.Schema);
                }

                yield return endpoint;
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