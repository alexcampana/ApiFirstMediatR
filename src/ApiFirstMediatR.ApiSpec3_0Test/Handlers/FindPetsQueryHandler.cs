namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers;

public sealed class FindPetsQueryHandler : IRequestHandler<FindPetsQuery, List<Pet>>
{
    public Task<List<Pet>> Handle(FindPetsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}