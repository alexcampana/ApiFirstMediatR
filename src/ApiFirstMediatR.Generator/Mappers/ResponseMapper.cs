using Microsoft.OpenApi.Expressions;

namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class ResponseMapper : IResponseMapper
{
    private readonly ITypeMapper _typeMapper;
    private readonly IOperationNamingRepository _operationNamingRepository;

    public ResponseMapper(ITypeMapper typeMapper, IOperationNamingRepository operationNamingRepository)
    {
        _typeMapper = typeMapper;
        _operationNamingRepository = operationNamingRepository;
    }

    public Response Map(OpenApiResponses responses)
    {
        // TODO: check if there is more than one 2xx response and register an unsupported warning diagnostic
        // TODO: refactor this so it's more extensible
        if (responses.TryGetValue("200", out var successResponse))
        {
            return Map200(successResponse);
        }
        else if (responses.TryGetValue("201", out successResponse))
        {
            return Map201(successResponse);
        }
        else if (responses.TryGetValue("204", out successResponse))
        {
            return Map204(successResponse);
        }
        else
        {
            throw new NotSupportedException($"Supported response status not found.");
        }
    }

    private Response Map200(OpenApiResponse successResponse)
    {
        if (successResponse.Content.TryGetValue("application/json", out var responseBody))
        {
            return new Response
            {
                BodyType = _typeMapper.Map(responseBody.Schema),
                Description = successResponse.Description.SplitOnNewLine(),
                HttpStatusCode = HttpStatusCodes.Status200
            };
        }
        else
        {
            throw new NotSupportedException($"Only application/json response body supported.");
        }
    }

    private Response Map201(OpenApiResponse successResponse)
    {
        if (successResponse.Links.Any() && successResponse.Links.Count == 1)
        {
            if (successResponse.Content.TryGetValue("application/json", out var responseBody))
            {
                var link = successResponse.Links.Single();
                
                if (link.Value.OperationId is null)
                {
                    throw new NotImplementedException("Only links with valid operationIds are allowed.");
                }
                
                var routeValueDict = new Dictionary<string, string>();

                foreach (var param in link.Value.Parameters)
                {
                    if (param.Value.Expression is ResponseExpression)
                    {
                        var expression = param.Value.Expression as ResponseExpression;
                        if (expression?.Source is BodyExpression)
                        {
                            var bodyReference = expression.Source as BodyExpression;
                            routeValueDict.Add(param.Key, $"response.{bodyReference?.Fragment.ToCleanName().ToPascalCase()}"); // TODO: make this more configurable
                        }
                    }
                }
                
                return new CreatedResponse
                {
                    BodyType = _typeMapper.Map(responseBody.Schema),
                    Description = successResponse.Description.SplitOnNewLine(),
                    HttpStatusCode = HttpStatusCodes.Status201,
                    EndpointName = _operationNamingRepository.GetOperationNameByOperationId(link.Value.OperationId) ?? throw new NotSupportedException("OperationId must reference a valid operation"),
                    ControllerName = _operationNamingRepository.GetControllerNameByOperationId(link.Value.OperationId) ?? throw new NotSupportedException("OperationId must reference a valid operation"),
                    RouteMaps = routeValueDict
                };
            }
            else
            {
                throw new NotSupportedException($"Only application/json response body supported.");
            }
        }
        else
        {
            throw new NotSupportedException("201 Created response needs one and only 1 link defined.");
        }
    }

    private Response Map204(OpenApiResponse successResponse)
    {
        return new Response
        {
            BodyType = null,
            Description = successResponse.Description.SplitOnNewLine(),
            HttpStatusCode = HttpStatusCodes.Status204
        };
    }
}