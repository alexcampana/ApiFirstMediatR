namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Pets;

public class GetPetByIdQueryHandler : IRequestHandler<GetPetByIdQuery, Pet>
{
    public Task<Pet> Handle(GetPetByIdQuery request, CancellationToken cancellationToken)
    {
        var pet = TestData
            .Pets
            .FirstOrDefault(p => p.Id == request.PetId);

        return Task.FromResult(pet)!;
    }
}