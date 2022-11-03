namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers;

public sealed class AddPetCommandHandler : IRequestHandler<AddPetCommand, Pet>
{
    public Task<Pet> Handle(AddPetCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}