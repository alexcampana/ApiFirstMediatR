namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers;

public sealed class FindPetsQueryHandler : IRequestHandler<FindPetsQuery, IEnumerable<Pet>>
{
    public Task<IEnumerable<Pet>> Handle(FindPetsQuery request, CancellationToken cancellationToken)
    {
        var pets = new List<Pet>
        {
            new Pet
            {
                Id = 1,
                Name = "Pet1",
                Tag = "PetTag1"
            },
            new Pet
            {
                Id = 2,
                Name = "Pet2"
            }
        };
        
        return Task.FromResult(pets as IEnumerable<Pet>);
    }
}