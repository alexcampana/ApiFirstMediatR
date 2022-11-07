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
                    MediatorRequestName = endpointName.ToPascalCase() + (operation.Key == OperationType.Get ? "Query" : "Command"),
                    Description = operation.Value.Description?.SplitOnNewLine(),
                    ResponseBodyType = "Unit",
                    QueryParameters = queryParams,
                    PathParameters = pathParams
                };

                if (operation.Value.RequestBody is not null &&
                    operation.Value.RequestBody.Content.TryGetValue("application/json", out var requestBody))
                {
                    endpoint.RequestBody = new Parameter
                    {
                        ParameterName = "body", // TODO: Make this configurable by end user
                        Name = "Body",
                        JsonName = "body",
                        Description = operation.Value.RequestBody.Description?.SplitOnNewLine(),
                        DataType = TypeMapper.Map(requestBody.Schema),
                        IsNullable = !operation.Value.RequestBody.Required,
                        Attribute = "[Microsoft.AspNetCore.Mvc.FromBody]"
                    };
                }
                else
                {
                    // TODO: Throw unsupported diagnostic
                }

                if (operation.Value.Responses.TryGetValue("200", out var successResponse))
                {
                    if (successResponse.Content.TryGetValue("application/json", out var responseBody))
                    {
                        endpoint.ResponseBodyType = TypeMapper.Map(responseBody.Schema);
                        endpoint.ResponseDescription = successResponse.Description.SplitOnNewLine();
                    }
                    else
                    {
                        // TODO: Throw unsupported diagnostic
                    }
                }
                else
                {
                    // TODO: Throw unsupported diagnostic
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