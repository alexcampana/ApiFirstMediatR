namespace ApiFirstMediatR.Generator.Tests.Repositories;

public class OperationNamingRepositoryTests
{
    [Fact]
    public void ValidAPISpec_OperationId_HappyPath()
    {
        var mockApiSpecRepository = new Mock<IApiSpecRepository>();
        mockApiSpecRepository
            .Setup(mock => mock.Get())
            .Returns(new OpenApiDocument
            {
                Paths = new OpenApiPaths
                {
                    ["/api/HelloWorld"] = new OpenApiPathItem
                    {
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            [OperationType.Get] = new OpenApiOperation
                            {
                                Description = "Hello World!",
                                OperationId = "GetHelloWorld",
                                Responses = new OpenApiResponses
                                {
                                    ["200"] = new OpenApiResponse
                                    {
                                        Description = "Ok"
                                    }
                                }
                            }
                        }
                    }
                }
            });

        var operationNamingRepository = new OperationNamingRepository(mockApiSpecRepository.Object);
        
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
}