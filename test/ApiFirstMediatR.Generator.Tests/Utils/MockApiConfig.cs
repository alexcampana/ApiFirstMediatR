namespace ApiFirstMediatR.Generator.Tests.Utils;

internal static class MockApiConfig
{
    public static IApiConfigRepository Create(string namespaceName = "Test")
    {
        var mockApiConfigRepo = Substitute.For<IApiConfigRepository>();
        mockApiConfigRepo
            .Get()
            .Returns(new ApiConfig
            {
                Namespace = namespaceName,
                SerializationLibrary = SerializationLibrary.SystemTextJson,
                RequestBodyName = "Body"
            });

        return mockApiConfigRepo;
    }
}