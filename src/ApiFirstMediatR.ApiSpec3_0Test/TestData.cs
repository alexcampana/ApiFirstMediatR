namespace ApiFirstMediatR.ApiSpec3_0Test;

public class TestData
{
    private static readonly Category DogCategory = new Category
    {
        Id = 1,
        Name = "Dog"
    };

    private static readonly Category CatCategory = new Category
    {
        Id = 2,
        Name = "Cat"
    };

    private static readonly Tag FriendlyTag = new Tag
    {
        Id = 1,
        Name = "Friendly"
    };

    private static readonly Tag DeviousTag = new Tag
    {
        Id = 2,
        Name = "Devious"
    };

    private static readonly Pet Pet1 = new Pet
    {
        Id = 1,
        Name = "Pet1",
        Category = DogCategory,
        Tags = new[]
        {
            FriendlyTag
        },
        Status = PetStatus.Available
    };

    private static readonly Pet Pet2 = new Pet
    {
        Id = 2,
        Name = "Pet2",
        Category = CatCategory,
        Tags = new[]
        {
            DeviousTag
        },
        Status = PetStatus.Available
    };

    private static readonly Order Order1 = new Order
    {
        Id = 1,
        PetId = 1,
        Quantity = 1,
        Complete = true,
        ShipDate = DateTimeOffset.Now,
        Status = OrderStatus.Delivered
    };
    
    public static readonly ImmutableArray<Category> Categories = ImmutableArray.Create(DogCategory, CatCategory);
    
    public static readonly ImmutableArray<Tag> Tags = ImmutableArray.Create(FriendlyTag, DeviousTag);
    
    public static readonly ImmutableArray<Pet> Pets = ImmutableArray.Create(Pet1, Pet2);
}