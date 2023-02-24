namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class EndpointMapper : IEndpointMapper
{
    private readonly IParameterMapper _parameterMapper;
    private readonly IResponseMapper _responseMapper;
    private readonly ITypeMapper _typeMapper;
    private readonly ISecurityMapper _securityMapper;
    private readonly IOperationNamingRepository _operationNamingRepository;
    private readonly IApiConfigRepository _apiConfigRepository;
    private readonly IDiagnosticReporter _diagnosticReporter;

    public EndpointMapper(
        IParameterMapper parameterMapper,
        IResponseMapper responseMapper,
        ITypeMapper typeMapper,
        ISecurityMapper securityMapper,
        IOperationNamingRepository operationNamingRepository,
        IApiConfigRepository apiConfigRepository,
        IDiagnosticReporter diagnosticReporter)
    {
        _parameterMapper = parameterMapper;
        _responseMapper = responseMapper;
        _typeMapper = typeMapper;
        _securityMapper = securityMapper;
        _operationNamingRepository = operationNamingRepository;
        _apiConfigRepository = apiConfigRepository;
        _diagnosticReporter = diagnosticReporter;
    }

    public IEnumerable<Endpoint> Map(OpenApiPaths paths)
    {
        var apiConfig = _apiConfigRepository.Get();
        var endpoints = new List<Endpoint>();
        
        foreach (var path in paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                try
                {
                    var endpointName = _operationNamingRepository.GetOperationNameByPathAndOperationType(path.Key, operation.Key);

                    var queryParams = _parameterMapper.Map(operation.Value.Parameters.Where(p => p.In == ParameterLocation.Query));
                    var pathParams = _parameterMapper.Map(operation.Value.Parameters.Where(p => p.In == ParameterLocation.Path));
                    // TODO: Add support for Cookie, Header params

                    var endpoint = new Endpoint
                    {
                        Name = endpointName,
                        Path = path.Key,
                        OperationName = operation.Key.GetDisplayName().ToPascalCase(),
                        MediatorRequestName = $"{endpointName}{(operation.Key == OperationType.Get ? "Query" : "Command")}",
                        Description = operation.Value.Description?.SplitOnNewLine(),
                        QueryParameters = queryParams,
                        PathParameters = pathParams
                    };

                    if (operation.Value.RequestBody is not null &&
                        operation.Value.RequestBody.Content.TryGetValue("application/json", out var requestBody))
                    {
                        endpoint.RequestBody = new Parameter
                        {
                            ParameterName = apiConfig.RequestBodyName.ToCamelCase(),
                            Name = apiConfig.RequestBodyName.ToPascalCase(),
                            JsonName = apiConfig.RequestBodyName.ToCamelCase(),
                            Description = operation.Value.RequestBody.Description?.SplitOnNewLine(),
                            DataType = _typeMapper.Map(requestBody.Schema),
                            IsNullable = !operation.Value.RequestBody.Required,
                            Attribute = "[Microsoft.AspNetCore.Mvc.FromBody]"
                        };
                    }
                    else if (operation.Value.RequestBody is not null)
                    {
                        throw new NotImplementedException($"Only application/json request body supported.");
                    }

                    endpoint.Response = _responseMapper.Map(operation.Value.Responses);
                    endpoint.Security = _securityMapper.Map(operation.Value.Security);
                    endpoints.Add(endpoint);
                }
                catch (Exception e) when (e is NotSupportedException or NotImplementedException)
                {
                    // TODO: Implement location finder so we can point to the exact location in the api spec file
                    _diagnosticReporter.ReportDiagnostic(DiagnosticCatalog.ApiSpecFeatureNotSupported(Location.None, $"{e.Message} Endpoint: {operation.Key.GetDisplayName().ToUpper()} {path.Key}"));
                }
                catch (Exception e)
                {
                    _diagnosticReporter.ReportDiagnostic(DiagnosticCatalog.ApiFirstMediatRUnexpectedError(Location.None, $"{e.Message} Endpoint: {operation.Key.GetDisplayName().ToUpper()} {path.Key}"));
                }
            }
        }

        return endpoints;
    }
}