namespace ApiFirstMediatR.Generator.IntegrationTests;

public class GetTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetTests(WebApplicationFactory<Program> application)
    {
        _client = application.CreateClient();
    }

    [Fact]
    public async Task Get_Success()
    {
        var response = await _client.GetAsync("/pet/categories");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categories = JsonConvert.DeserializeObject<List<Category>>(await response.Content.ReadAsStringAsync());

        categories.Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new []
            {
                new
                {
                    Id = 1,
                    Name = "Dog"
                },
                new
                {
                    Id = 2,
                    Name = "Cat"
                }
            });
    }

    [Fact]
    public async Task Get_WithPathParam_Success()
    {
        var response = await _client.GetAsync("/pet/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pet = JsonConvert.DeserializeObject<Pet>(await response.Content.ReadAsStringAsync());

        pet.Should().NotBeNull()
            .And.BeEquivalentTo(new
            {
                Id = 1,
                Name = "Pet1",
                Category = new
                {
                    Id = 1,
                    Name = "Dog"
                },
                Tags = new[]
                {
                    new
                    {
                        Id = 1,
                        Name = "Friendly"
                    }
                },
                Status = PetStatus.Available
            });
    }

    [Fact]
    public async Task Get_WithSerializedEnum_CorrectSerialization()
    {
        var response = await _client.GetAsync("/pet/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rawJson = await response.Content.ReadAsStringAsync();

        rawJson.Should().Be("{\"id\":1,\"name\":\"Pet1\",\"category\":{\"id\":1,\"name\":\"Dog\"},\"photoUrls\":null,\"tags\":[{\"id\":1,\"name\":\"Friendly\"}],\"status\":\"available\"}");
    }

    [Fact]
    public async Task Get_WithPathParamAnd_NoContent()
    {
        var response = await _client.GetAsync("/pet/3");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}