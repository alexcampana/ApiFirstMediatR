namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class ControllerMapperTests
{
    private readonly IControllerMapper _controllerMapper;

    public ControllerMapperTests()
    {
        var mockApiConfigRepo = MockApiConfig.Create();
        var mockOperationNamingRepository = new Mock<IOperationNamingRepository>();
        mockOperationNamingRepository
            .Setup(mock => mock.GetControllerNameByPath("/test"))
            .Returns("Test");

        var mockDiagnosticReporter = new Mock<IDiagnosticReporter>();
        var typeMapper = new TypeMapper(mockApiConfigRepo);
        var parameterMapper = new ParameterMapper(typeMapper);
        var responseMapper = new ResponseMapper(typeMapper, mockOperationNamingRepository.Object);
        var securityMapper = new SecurityMapper(mockDiagnosticReporter.Object);
        var mockApiConfigRepository = new Mock<IApiConfigRepository>();

        var endpointMapper = new EndpointMapper(parameterMapper, responseMapper, typeMapper, securityMapper,
            mockOperationNamingRepository.Object, mockApiConfigRepository.Object, mockDiagnosticReporter.Object);

        _controllerMapper = new ControllerMapper(endpointMapper, mockOperationNamingRepository.Object);
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

        var controllers = _controllerMapper.Map(apiSpec).ToList();
        controllers.Should().ContainSingle();
        
        var controller = controllers.Single();
        controller.Name.Should().Be("TestController");
        controller.Endpoints.Should().HaveCount(1);
    }
}