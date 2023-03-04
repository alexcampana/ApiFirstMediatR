namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class EndpointMapperTests
{
    private readonly IEndpointMapper _endpointMapper;
    private readonly Mock<IDiagnosticReporter> _mockDiagnosticReporter;

    public EndpointMapperTests()
    {
        var mockApiConfigRepo = MockApiConfig.Create();
        var mockOperationNamingRepository = new Mock<IOperationNamingRepository>();
        mockOperationNamingRepository
            .Setup(mock => mock.GetControllerNameByOperationId("TestOperation"))
            .Returns("TestController");

        mockOperationNamingRepository
            .Setup(mock => mock.GetOperationNameByOperationId("TestOperation"))
            .Returns("TestOperation");

        mockOperationNamingRepository
            .Setup(mock => mock.GetOperationNameByPathAndOperationType("/test", OperationType.Get))
            .Returns("TestOperation");

        _mockDiagnosticReporter = new Mock<IDiagnosticReporter>();
        var typeMapper = new TypeMapper(mockApiConfigRepo);
        var parameterMapper = new ParameterMapper(typeMapper);
        var responseMapper = new ResponseMapper(typeMapper, mockOperationNamingRepository.Object);
        var securityMapper = new SecurityMapper(_mockDiagnosticReporter.Object);
        var mockApiConfigRepository = new Mock<IApiConfigRepository>();

        _endpointMapper = new EndpointMapper(parameterMapper, responseMapper, typeMapper, securityMapper,
            mockOperationNamingRepository.Object, mockApiConfigRepository.Object, _mockDiagnosticReporter.Object);
    }

    [Fact]
    public void ValidEndpointPath_HappyPath()
    {
        var paths = new OpenApiPaths
        {
            ["/test"] = new OpenApiPathItem
            {
                Operations = new Dictionary<OperationType, OpenApiOperation>
                {
                    [OperationType.Get] = new OpenApiOperation
                    {
                        OperationId = "TestOperation",
                        Description = "Test Description",
                        Summary = "Test Summary",
                        Parameters = new List<OpenApiParameter>
                        {
                            new OpenApiParameter
                            {
                                Name = "TestParameter",
                                In = ParameterLocation.Query,
                                Description = "Test Parameter Description",
                                Schema = new OpenApiSchema
                                {
                                    Type = "integer"
                                }
                            }
                        },
                        Responses = new OpenApiResponses
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
                        },
                        Security = new List<OpenApiSecurityRequirement>
                        {
                            new()
                            {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Type = SecuritySchemeType.Http,
                                        Scheme = "bearer",
                                        BearerFormat = "JWT"
                                    },
                                    new List<string>
                                    {
                                        "GetResource"
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var endpoints = _endpointMapper.Map(paths).ToList();

        endpoints.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                Name = "TestOperation",
                Path = "/test",
                OperationName = "Get",
                MediatrRequestName = "TestOperationQuery",
                Description = new []
                {
                    "Test Description"
                },
                Security = new Security
                {
                    Policies = new []
                    {
                        "GetResource"
                    }
                }
            }, options => options.ExcludingMissingMembers());

        var endpoint = endpoints.Single();

        endpoint.Response.Should().NotBeNull();
        endpoint.Response!.HttpStatusCode.Should().Be(HttpStatusCodes.Status200);
        endpoint.RequestBody.Should().BeNull();
        endpoint.QueryParameters.Should().ContainSingle()
            .Which.Name.Should().Be("TestParameter");
        endpoint.PathParameters.Should().BeNullOrEmpty();
        endpoint.Security.Should().NotBeNull();
        endpoint.Security!.Policies.Should().ContainSingle();
    }

    [Fact]
    public void InvalidRequestBody_ThrowsDiagnostic()
    {
        var paths = new OpenApiPaths
        {
            ["/test"] = new OpenApiPathItem
            {
                Operations = new Dictionary<OperationType, OpenApiOperation>
                {
                    [OperationType.Post] = new OpenApiOperation
                    {
                        OperationId = "TestOperation",
                        Description = "Test Description",
                        Summary = "Test Summary",
                        Parameters = new List<OpenApiParameter>
                        {
                            new OpenApiParameter
                            {
                                Name = "TestParameter",
                                In = ParameterLocation.Query,
                                Description = "Test Parameter Description",
                                Schema = new OpenApiSchema
                                {
                                    Type = "integer"
                                }
                            }
                        },
                        RequestBody = new OpenApiRequestBody
                        {
                            Content = new Dictionary<string, OpenApiMediaType>
                            {
                                
                                ["application/xml"] = new OpenApiMediaType
                                {
                                    
                                }
                            }
                        },
                        Responses = new OpenApiResponses
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
                        }
                    }
                }
            }
        };
        
        var endpoints = _endpointMapper.Map(paths);
        endpoints.Should().BeEmpty();
        _mockDiagnosticReporter.Verify(m => m.ReportDiagnostic(It.IsAny<Diagnostic>()));
        _mockDiagnosticReporter.VerifyNoOtherCalls();
    }

    [Fact]
    public void ValidEndpointPath_WithPathQueryParameter()
    {
        var paths = new OpenApiPaths
        {
            ["/test"] = new OpenApiPathItem
            {
                Parameters = new List<OpenApiParameter>
                {
                    new OpenApiParameter
                    {
                        Name = "TestParameter",
                        In = ParameterLocation.Query,
                        Description = "Test Parameter Description",
                        Schema = new OpenApiSchema
                        {
                            Type = "integer"
                        }
                    }
                },
                Operations = new Dictionary<OperationType, OpenApiOperation>
                {
                    [OperationType.Get] = new OpenApiOperation
                    {
                        OperationId = "TestOperation",
                        Description = "Test Description",
                        Summary = "Test Summary",
                        Parameters = new List<OpenApiParameter>(),
                        Responses = new OpenApiResponses
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
                        },
                        Security = new List<OpenApiSecurityRequirement>
                        {
                            new()
                            {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Type = SecuritySchemeType.Http,
                                        Scheme = "bearer",
                                        BearerFormat = "JWT"
                                    },
                                    new List<string>
                                    {
                                        "GetResource"
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var endpoints = _endpointMapper.Map(paths).ToList();

        endpoints.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                Name = "TestOperation",
                Path = "/test",
                OperationName = "Get",
                MediatrRequestName = "TestOperationQuery",
                Description = new []
                {
                    "Test Description"
                },
                Security = new Security
                {
                    Policies = new []
                    {
                        "GetResource"
                    }
                }
            }, options => options.ExcludingMissingMembers());

        var endpoint = endpoints.Single();

        endpoint.Response.Should().NotBeNull();
        endpoint.Response!.HttpStatusCode.Should().Be(HttpStatusCodes.Status200);
        endpoint.RequestBody.Should().BeNull();
        endpoint.QueryParameters.Should().ContainSingle()
            .Which.Name.Should().Be("TestParameter");
        endpoint.PathParameters.Should().BeNullOrEmpty();
        endpoint.Security.Should().NotBeNull();
        endpoint.Security!.Policies.Should().ContainSingle();
    }
}