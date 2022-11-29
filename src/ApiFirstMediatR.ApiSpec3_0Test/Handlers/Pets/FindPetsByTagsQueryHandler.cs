namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Pets;

public class FindPetsByTagsQueryHandler : IRequestHandler<FindPetsByTagsQuery, IEnumerable<Pet>>
{
    public Task<IEnumerable<Pet>> Handle(FindPetsByTagsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}