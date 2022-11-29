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

        categories.Should().NotBeNull();
        categories.Should().HaveCount(2);

        var firstCategory = categories.First();
        firstCategory.Id.Should().Be(1);
        firstCategory.Name.Should().Be("Dog");

        var secondCategory = categories[1];
        secondCategory.Id.Should().Be(2);
        secondCategory.Name.Should().Be("Cat");
    }

    [Fact]
    public async Task Get_WithPathParam_Success()
    {
        var response = await _client.GetAsync("/pet/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pet = JsonConvert.DeserializeObject<Pet>(await response.Content.ReadAsStringAsync());
        
        pet.Should().NotBeNull();
        pet.Id.Should().Be(1);
        pet.Name.Should().Be("Pet1");
        pet.Category.Should().NotBeNull();
        pet.Category!.Id.Should().Be(1);
        pet.Category.Name.Should().Be("Dog");
        pet.Tags.Should().NotBeNull();
        pet.Tags.Should().HaveCount(1);
        pet.Tags.Should().ContainSingle();
        pet.Tags!.First().Id.Should().Be(1);
        pet.Tags!.First().Name.Should().Be("Friendly");
        pet.Status.Should().Be("available");
    }

    [Fact]
    public async Task Get_WithPathParamAnd_NoContent()
    {
        var response = await _client.GetAsync("/pet/3");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}