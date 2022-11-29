namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Pets;

public sealed class AddPetCommandHandler : IRequestHandler<AddPetCommand, Pet>
{
    public Task<Pet> Handle(AddPetCommand request, CancellationToken cancellationToken)
    {
        var pet = new Pet
        {
            Id = 3,
            Name = request.Body.Name,
            Category = request.Body.Category,
            PhotoUrls = request.Body.PhotoUrls,
            Tags = request.Body.Tags,
            Status = request.Body.Status
        };

        return Task.FromResult(pet);
    }
}