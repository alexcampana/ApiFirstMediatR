namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers;

public sealed class FindPetsQueryHandler : IRequestHandler<FindPetsQuery, IEnumerable<Pet>>
{
    public Task<IEnumerable<Pet>> Handle(FindPetsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}