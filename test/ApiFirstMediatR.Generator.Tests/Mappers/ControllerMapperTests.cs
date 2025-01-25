namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class ControllerMapperTests
{
    private readonly IControllerMapper _controllerMapper;

    public ControllerMapperTests()
    {
        var mockApiConfigRepo = MockApiConfig.Create();
        var mockOperationNamingRepository = Substitute.For<IOperationNamingRepository>();
        mockOperationNamingRepository
            .GetControllerNameByPath("/test")
            .Returns("Test");

        var mockDiagnosticReporter = Substitute.For<IDiagnosticReporter>();
        var typeMapper = new TypeMapper(mockApiConfigRepo);
        var parameterMapper = new ParameterMapper(typeMapper);
        var responseMapper = new ResponseMapper(typeMapper, mockOperationNamingRepository);
        var securityMapper = new SecurityMapper(mockDiagnosticReporter);
        var mockApiConfigRepository = Substitute.For<IApiConfigRepository>();

        var endpointMapper = new EndpointMapper(parameterMapper, responseMapper, typeMapper, securityMapper,
            mockOperationNamingRepository, mockApiConfigRepository, mockDiagnosticReporter);

        _controllerMapper = new ControllerMapper(endpointMapper, mockOperationNamingRepository);
    }

    [Fact]
    public void ValidController_HappyPath()
    {
        var apiSpec = new OpenApiDocument
        {
            Paths = new OpenApiPaths
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
                            }
                        }
                    }
                }
            }
        };

        var controllers = _controllerMapper.Map(new []{ apiSpec }).ToList();
        controllers.Should().ContainSingle();
        
        var controller = controllers.Single();
        controller.Name.Should().Be("TestController");
        controller.Endpoints.Should().HaveCount(1);
    }
}