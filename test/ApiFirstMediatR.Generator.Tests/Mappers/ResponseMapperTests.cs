namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class ResponseMapperTests
{
    private readonly ITypeMapper _typeMapper;
    private readonly IOperationNamingRepository _operationNamingRepository;
    private readonly IResponseMapper _responseMapper;
    
    public ResponseMapperTests()
    {
        var mockApiConfigRepo = MockApiConfig.Create();
        var mockOperationNamingRepository = new Mock<IOperationNamingRepository>();
        mockOperationNamingRepository
            .Setup(mock => mock.GetControllerNameByOperationId("TestOperation"))
            .Returns("TestController");

        mockOperationNamingRepository
            .Setup(mock => mock.GetOperationNameByOperationId("TestOperation"))
            .Returns("TestOperation");

        _typeMapper = new TypeMapper(mockApiConfigRepo);
        _operationNamingRepository = mockOperationNamingRepository.Object;
        _responseMapper = new ResponseMapper(_typeMapper, _operationNamingRepository);
    }

    [Fact]
    public void Valid200Response_HappyPath()
    {
        var responses = new OpenApiResponses
        {
            ["200"] = new OpenApiResponse
            {
                Description = "Ok",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "Boolean"
                        }
                    }
                }
            }
        };

        var result = _responseMapper.Map(responses);
        
        Assert.NotNull(result);
        Assert.Equal("bool", result.BodyType);
        Assert.Equal(HttpStatusCodes.Status200, result.HttpStatusCode);
    }

    [Fact]
    public void Valid201Response_HappyPath()
    {
        var responses = new OpenApiResponses
        {
            ["201"] = new OpenApiResponse
            {
                Description = "Created",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "integer"
                        }
                    }
                },
                Links = new Dictionary<string, OpenApiLink>
                {
                    ["CreatedLink"] = new OpenApiLink
                    {
                        OperationId = "TestOperation",
                        Parameters = new Dictionary<string, RuntimeExpressionAnyWrapper>
                        {
                            ["createdId"] = new RuntimeExpressionAnyWrapper
                            {
                                Expression = new ResponseExpression(new BodyExpression(new JsonPointer("$response.body#/id")))
                            }
                        }
                    }
                }
            }
        };

        var result = _responseMapper.Map(responses);
        
        Assert.NotNull(result);
        Assert.Equal("int", result.BodyType);
        Assert.Equal(HttpStatusCodes.Status201, result.HttpStatusCode);
        Assert.IsType<CreatedResponse>(result);
        var createdResponse = result as CreatedResponse;
        Assert.Equal("TestOperation", createdResponse!.EndpointName);
        Assert.Equal("TestController", createdResponse.ControllerName);
        Assert.Contains("createdId", createdResponse.RouteMaps as IDictionary<string, string>);
        Assert.Equal("response.Id", createdResponse.RouteMaps["createdId"]);
    }
    
    [Fact]
    public void Valid204Response_HappyPath()
    {
        var responses = new OpenApiResponses
        {
            ["204"] = new OpenApiResponse
            {
                Description = "No Content"
            }
        };

        var result = _responseMapper.Map(responses);
        
        Assert.NotNull(result);
        Assert.Null(result.BodyType);
        Assert.Equal(HttpStatusCodes.Status204, result.HttpStatusCode);
    }

    [Fact]
    public void NoResponses_ThrowsNotSupported()
    {
        var responses = new OpenApiResponses();
        Assert.Throws<NotSupportedException>(() => _responseMapper.Map(responses));
    }

    [Fact]
    public void ImALittleTeaPotResponse_ThrowsNotSupported()
    {
        var responses = new OpenApiResponses
        {
            ["418"] = new OpenApiResponse
            {
                Description = "I'm a little teapot"
            }
        };
        Assert.Throws<NotSupportedException>(() => _responseMapper.Map(responses));
    }

    [Fact]
    public void Valid201Response_WithMultipleLinks_ThrowsNotSupported()
    {
        var responses = new OpenApiResponses
        {
            ["201"] = new OpenApiResponse
            {
                Description = "Created",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "integer"
                        }
                    }
                },
                Links = new Dictionary<string, OpenApiLink>
                {
                    ["CreatedLink"] = new OpenApiLink
                    {
                        OperationId = "TestOperation",
                        Parameters = new Dictionary<string, RuntimeExpressionAnyWrapper>
                        {
                            ["createdId"] = new RuntimeExpressionAnyWrapper
                            {
                                Expression = new ResponseExpression(new BodyExpression(new JsonPointer("$response.body#/id")))
                            }
                        }
                    },
                    ["AnotherLink"] = new OpenApiLink
                    {
                        OperationId = "AnotherOperation",
                        Parameters = new Dictionary<string, RuntimeExpressionAnyWrapper>
                        {
                            ["createdId"] = new RuntimeExpressionAnyWrapper
                            {
                                Expression = new ResponseExpression(new BodyExpression(new JsonPointer("$response.body#/id")))
                            }
                        }
                    }
                }
            }
        };

        Assert.Throws<NotSupportedException>(() => _responseMapper.Map(responses));
    }
    
    [Fact]
    public void Valid201Response_WithNoLinks_ThrowsNotSupported()
    {
        var responses = new OpenApiResponses
        {
            ["201"] = new OpenApiResponse
            {
                Description = "Created",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "integer"
                        }
                    }
                },
                Links = new Dictionary<string, OpenApiLink>()
            }
        };

        Assert.Throws<NotSupportedException>(() => _responseMapper.Map(responses));
    }

    [Fact]
    public void Valid200Response_WithTextPlainResponse_ThrowsNotSupported()
    {
        var responses = new OpenApiResponses
        {
            ["200"] = new OpenApiResponse
            {
                Description = "Ok",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["text/plain"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "Boolean"
                        }
                    }
                }
            }
        };

        Assert.Throws<NotSupportedException>(() => _responseMapper.Map(responses));
    }
}