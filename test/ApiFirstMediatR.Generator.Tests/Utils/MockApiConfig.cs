namespace ApiFirstMediatR.Generator.Tests.Utils;

internal static class MockApiConfig
{
    public static IApiConfigRepository Create(string namespaceName = "Test")
    {
        var mockApiConfigRepo = new Mock<IApiConfigRepository>();
        mockApiConfigRepo
            .Setup(mock => mock.Get())
            .Returns(new ApiConfig
            {
                Namespace = namespaceName,
                SerializationLibrary = SerializationLibrary.SystemTextJson,
                RequestBodyName = "Body"
            });

        return mockApiConfigRepo.Object;
    }
}