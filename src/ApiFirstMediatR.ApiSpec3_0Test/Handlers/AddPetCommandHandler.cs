namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers;

public sealed class AddPetCommandHandler : IRequestHandler<AddPetCommand, Pet>
{
    public Task<Pet> Handle(AddPetCommand request, CancellationToken cancellationToken)
    {
        var pet = new Pet
        {
            Id = 1,
            Name = request.Body.Name,
            Tag = request.Body.Tag
        };

        return Task.FromResult(pet);
    }
}