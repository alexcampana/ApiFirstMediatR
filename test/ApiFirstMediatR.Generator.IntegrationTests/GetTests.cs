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
        var response = await _client.GetAsync("/pets");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pets = JsonConvert.DeserializeObject<List<Pet>>(await response.Content.ReadAsStringAsync());

        pets.Should().NotBeNull();
        pets.Count.Should().Be(2);
        
        var firstPet = pets.First();
        firstPet.Id.Should().Be(1);
        firstPet.Name.Should().Be("Pet1");
        firstPet.Tag.Should().Be("PetTag1");

        var secondPet = pets[1];
        secondPet.Id.Should().Be(2);
        secondPet.Name.Should().Be("Pet2");
        secondPet.Tag.Should().BeNull();
    }

    [Fact]
    public async Task Get_WithPathParam_Success()
    {
        var response = await _client.GetAsync("/pets/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pet = JsonConvert.DeserializeObject<Pet>(await response.Content.ReadAsStringAsync());
        
        pet.Should().NotBeNull();
        pet.Id.Should().Be(1);
        pet.Name.Should().Be("Pet1");
        pet.Tag.Should().Be("PetTag1");
    }

    [Fact]
    public async Task Get_WithPathParamAnd_NoContent()
    {
        var response = await _client.GetAsync("/pets/3");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}