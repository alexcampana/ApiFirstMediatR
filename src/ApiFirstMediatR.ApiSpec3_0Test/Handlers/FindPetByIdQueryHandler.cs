namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers;

public class FindPetByIdQueryHandler : IRequestHandler<FindPetByIdQuery, Pet>
{
    public Task<Pet> Handle(FindPetByIdQuery request, CancellationToken cancellationToken)
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

        var pet = pets
            .FirstOrDefault(p => p.Id == request.Id);

        return Task.FromResult(pet)!;
    }
}