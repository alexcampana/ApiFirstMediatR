namespace ApiFirstMediatR.Generator.Tests.Utils;

internal static class MockApiConfig
{
    public static IApiConfigRepository Create(string namespaceName = "Test",
        OperationGenerationMode operationGenerationMode = OperationGenerationMode.MultipleClientsFromPathSegmentAndOperationId)
    {
        var mockApiConfigRepo = new Mock<IApiConfigRepository>();
        mockApiConfigRepo
            .Setup(mock => mock.Get())
            .Returns(new ApiConfig
            {
                Namespace = namespaceName,
                SerializationLibrary = SerializationLibrary.SystemTextJson,
                RequestBodyName = "Body",
                OperationGenerationMode = operationGenerationMode,
            });

        return mockApiConfigRepo.Object;
    }
}