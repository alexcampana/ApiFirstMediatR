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
        var pet = new Pet
        {
            Name = "Pet Name",
            Category = new Category
            {
                Id = 1,
                Name = "Dog"
            },
        };

        var json = JsonConvert.SerializeObject(pet);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/pet", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responsePet = JsonConvert.DeserializeObject<Pet>(await response.Content.ReadAsStringAsync());

        responsePet.Should().NotBeNull()
            .And.BeEquivalentTo(new
            {
                Id = 3,
                Name = "Pet Name",
                Category = new
                {
                    Id = 1,
                    Name = "Dog"
                }
            });
    }

    [Fact]
    public async Task Post_Created()
    {
        var order = new Order
        {
            PetId = 1,
            Quantity = 1,
            Status = OrderStatus.Placed
        };

        var json = JsonConvert.SerializeObject(order);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/store/order", content);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull()
            .And.Be("http://localhost/store/order/1");

        var orderPlaced = JsonConvert.DeserializeObject<OrderPlaced>(await response.Content.ReadAsStringAsync());
        orderPlaced!.Id.Should().Be(1);
    }
}