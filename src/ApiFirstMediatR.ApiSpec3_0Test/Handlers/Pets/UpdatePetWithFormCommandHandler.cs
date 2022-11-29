namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Pets;

public class UpdatePetWithFormCommandHandler : IRequestHandler<UpdatePetWithFormCommand, Pet>
{
    public Task<Pet> Handle(UpdatePetWithFormCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}