namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Pets;

public class UpdatePetCommandHandler : IRequestHandler<UpdatePetCommand, Pet>
{
    public Task<Pet> Handle(UpdatePetCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}