namespace ApiFirstMediatR.Generator.Tests.Repositories;

public class OperationNamingRepositoryTests
{
    [Fact]
    public void ValidAPISpec_MultipleClientsFromPathSegmentAndOperationId_HappyPath()
    {
        var mockApiSpecRepository = new Mock<IApiSpecRepository>();
        var mockApiConfigRepository = MockApiConfig.Create(operationGenerationMode: OperationGenerationMode.MultipleClientsFromPathSegmentAndOperationId);
        
        mockApiSpecRepository
            .Setup(mock => mock.Get())
            .Returns(GetOpenApiDocument());

        var operationNamingRepository = new OperationNamingRepository(mockApiSpecRepository.Object, mockApiConfigRepository);
        
        const string expectedControllerName = "Api";
        const string expectedOperationName = "GetHelloWorld";

        operationNamingRepository.GetControllerNameByPath("/api/HelloWorld")
            .Should().Be(expectedControllerName);
        operationNamingRepository.GetOperationNameByPathAndOperationType("/api/HelloWorld", OperationType.Get)
            .Should().Be(expectedOperationName);
        operationNamingRepository.GetControllerNameByOperationId("GetHelloWorld")
            .Should().Be(expectedControllerName);
        operationNamingRepository.GetOperationNameByOperationId("GetHelloWorld")
            .Should().Be(expectedOperationName);
    }
    
    [Fact]
    public void ValidAPISpec_MultipleClientsFromFirstTagAndOperationId_HappyPath()
    {
        var mockApiSpecRepository = new Mock<IApiSpecRepository>();
        var mockApiConfigRepository = MockApiConfig.Create(operationGenerationMode: OperationGenerationMode.MultipleClientsFromFirstTagAndOperationId);
        
        mockApiSpecRepository
            .Setup(mock => mock.Get())
            .Returns(GetOpenApiDocument());

        var operationNamingRepository = new OperationNamingRepository(mockApiSpecRepository.Object, mockApiConfigRepository);
        
        const string expectedControllerName = "HelloWorld";
        const string expectedOperationName = "GetHelloWorld";

        operationNamingRepository.GetControllerNameByPath("/api/HelloWorld")
            .Should().Be(expectedControllerName);
        operationNamingRepository.GetOperationNameByPathAndOperationType("/api/HelloWorld", OperationType.Get)
            .Should().Be(expectedOperationName);
        operationNamingRepository.GetControllerNameByOperationId("GetHelloWorld")
            .Should().Be(expectedControllerName);
        operationNamingRepository.GetOperationNameByOperationId("GetHelloWorld")
            .Should().Be(expectedOperationName);
    }
    
    private static OpenApiDocument GetOpenApiDocument()
    {
        return new OpenApiDocument
        {
            Paths = new OpenApiPaths
            {
                ["/api/HelloWorld"] = new()
                {
                    Operations = new Dictionary<OperationType, OpenApiOperation>
                    {
                        [OperationType.Get] = new()
                        {
                            Tags = new List<OpenApiTag>
                            {
                                new() { Name = "HelloWorld" }
                            },
                            Description = "Hello World!",
                            OperationId = "GetHelloWorld",
                            Responses = new OpenApiResponses
                            {
                                ["200"] = new()
                                {
                                    Description = "Ok"
                                }
                            }
                        }
                    }
                }
            }
        };
    }
}