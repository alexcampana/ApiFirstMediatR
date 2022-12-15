using ApiFirstMediatR.ApiSpec2_0Test.Dtos;

namespace ApiFirstMediatR.Generator.IntegrationTests;

public class NewtonsoftTests : IClassFixture<WebApplicationFactory<ApiSpec2_0Test.Program>>
{
    private readonly HttpClient _client;

    public NewtonsoftTests(WebApplicationFactory<ApiSpec2_0Test.Program> application)
    {
        _client = application.CreateClient();
    }

    [Fact]
    public async Task Get_HappyPath()
    {
        var response = await _client.GetAsync("me");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var profile = JsonConvert.DeserializeObject<Profile>(await response.Content.ReadAsStringAsync());
        profile.Should().BeEquivalentTo(new
        {
            FirstName = "First",
            LastName = "Last",
            Email = "test@test.com"
        });
    }
}