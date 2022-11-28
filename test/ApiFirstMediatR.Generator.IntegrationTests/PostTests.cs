namespace ApiFirstMediatR.Generator.IntegrationTests;

public class PostTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PostTests(WebApplicationFactory<Program> application)
    {
        _client = application.CreateClient();
    }
    
    [Fact]
    public async Task Post_Success()
    {
        var pet = new NewPet
        {
            Name = "Pet Name",
            Tag = "Pet Tag"
        };

        var json = JsonConvert.SerializeObject(pet);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/pets", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responsePet = JsonConvert.DeserializeObject<Pet>(await response.Content.ReadAsStringAsync());
        responsePet.Id.Should().Be(1);
        responsePet.Name.Should().Be("Pet Name");
        responsePet.Tag.Should().Be("Pet Tag");
    }
    
    // TODO: Test 201 Created
}