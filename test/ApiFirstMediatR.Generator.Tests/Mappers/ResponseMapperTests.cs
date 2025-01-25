namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class ResponseMapperTests
{
    private readonly IResponseMapper _responseMapper;
    
    public ResponseMapperTests()
    {
        var mockApiConfigRepo = MockApiConfig.Create();
        var mockOperationNamingRepository = Substitute.For<IOperationNamingRepository>();
        mockOperationNamingRepository
            .GetControllerNameByOperationId("TestOperation")
            .Returns("TestController");

        mockOperationNamingRepository
            .GetOperationNameByOperationId("TestOperation")
            .Returns("TestOperation");

        var typeMapper = new TypeMapper(mockApiConfigRepo);
        var operationNamingRepository = mockOperationNamingRepository;
        
        _responseMapper = new ResponseMapper(typeMapper, operationNamingRepository);
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

        var result = _responseMapper.Map(responses, "default");

        result.Should().NotBeNull()
            .And.BeEquivalentTo(new
            {

                BodyType = "bool",
                HttpStatusCode = HttpStatusCodes.Status200
            });
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

        var result = _responseMapper.Map(responses, "default");

        result.Should().NotBeNull()
            .And.BeOfType<CreatedResponse>()
            .And.BeEquivalentTo(new
            {
                BodyType = "int",
                HttpStatusCode = HttpStatusCodes.Status201,
                EndpointName = "TestOperation",
                ControllerName = "TestController",
                RouteMaps = new[]
                {
                    new
                    {
                        Key = "createdId",
                        Value = "response.Id"
                    }
                }
            });
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

        var result = _responseMapper.Map(responses, "default");

        result.Should().NotBeNull();
        result.BodyType.Should().BeNull();
        result.HttpStatusCode.Should().Be(HttpStatusCodes.Status204);
    }

    [Fact]
    public void NoResponses_ThrowsNotSupported()
    {
        var responses = new OpenApiResponses();
        _responseMapper.Invoking(m => m.Map(responses, "default"))
            .Should().Throw<NotSupportedException>();
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
        _responseMapper.Invoking(m => m.Map(responses, "default"))
            .Should().Throw<NotSupportedException>();
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

        _responseMapper.Invoking(m => m.Map(responses, "default"))
            .Should().Throw<NotSupportedException>();
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

        _responseMapper.Invoking(m => m.Map(responses, "default"))
            .Should().Throw<NotSupportedException>();
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

        _responseMapper.Invoking(m => m.Map(responses, "default"))
            .Should().Throw<NotSupportedException>();
    }
}