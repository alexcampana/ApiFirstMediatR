namespace ApiFirstMediatR.Generator.IntegrationTests;

public class DeleteTests : IClassFixture<WebApplicationFactory<ApiSpec3_0Test.Program>>
{
    private readonly HttpClient _client;

    public DeleteTests(WebApplicationFactory<ApiSpec3_0Test.Program> application)
    {
        _client = application.CreateClient();
    }

    [Fact]
    public async Task Delete_NoContent()
    {
        var response = await _client.DeleteAsync("pet/1");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}