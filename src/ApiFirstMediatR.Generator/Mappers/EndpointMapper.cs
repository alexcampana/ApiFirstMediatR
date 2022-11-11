namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class EndpointMapper : IEndpointMapper
{
    private readonly IParameterMapper _parameterMapper;
    private readonly IResponseMapper _responseMapper;
    private readonly ITypeMapper _typeMapper;

    public EndpointMapper(IParameterMapper parameterMapper, IResponseMapper responseMapper, ITypeMapper typeMapper)
    {
        _parameterMapper = parameterMapper;
        _responseMapper = responseMapper;
        _typeMapper = typeMapper;
    }

    public IEnumerable<Endpoint> Map(OpenApiPaths paths)
    {
        var endpoints = new List<Endpoint>();
        foreach (var path in paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                try
                {
                    var endpointName = operation.Value.OperationId ?? $"{operation.Key.GetDisplayName()}{PathToEndpointName(path.Key)}";

                    var queryParams = _parameterMapper.Map(operation.Value.Parameters.Where(p => p.In == ParameterLocation.Query));
                    var pathParams = _parameterMapper.Map(operation.Value.Parameters.Where(p => p.In == ParameterLocation.Path));
                    // TODO: Add support for Cookie, Header params
                    
                    var endpoint = new Endpoint
                    {
                        Name = endpointName.ToCleanName().ToPascalCase(),
                        Path = path.Key,
                        OperationName = operation.Key.GetDisplayName().ToPascalCase(),
                        MediatorRequestName = endpointName.ToCleanName().ToPascalCase() + (operation.Key == OperationType.Get ? "Query" : "Command"),
                        Description = operation.Value.Description?.SplitOnNewLine(),
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
                            DataType = _typeMapper.Map(requestBody.Schema),
                            IsNullable = !operation.Value.RequestBody.Required,
                            Attribute = "[Microsoft.AspNetCore.Mvc.FromBody]"
                        };
                    }
                    else if (operation.Value.RequestBody is not null)
                    {
                        throw new NotImplementedException($"Only application/json request body supported. Endpoint: {operation.Key.GetDisplayName()} {path.Key}");
                    }

                    // TODO: Throw Unsupported Diagnostic if more than one success response is registered
                    endpoint.Response = _responseMapper.Map(operation.Value.Responses);

                    endpoints.Add(endpoint);
                }
                catch (Exception e)
                {
                    // TODO: Throw diagnostic
                }
            }
        }

        return endpoints;
    }

    private static string PathToEndpointName(string path)
    {
        var pathParts  = path
            .Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(ps => ps.Replace("{", "").Replace("}", "").ToPascalCase());

        return string.Join("", pathParts);
    }
}