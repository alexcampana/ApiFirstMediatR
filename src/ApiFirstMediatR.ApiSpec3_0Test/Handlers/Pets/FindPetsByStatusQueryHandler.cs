namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Pets;

public class FindPetsByStatusQueryHandler : IRequestHandler<FindPetsByStatusQuery, IEnumerable<Pet>>
{
    public Task<IEnumerable<Pet>> Handle(FindPetsByStatusQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}