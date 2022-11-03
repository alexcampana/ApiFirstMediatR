namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers;

public class FindPetByIdQueryHandler : IRequestHandler<FindPetByIdQuery, Pet>
{
    public Task<Pet> Handle(FindPetByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}