using ApiFirstMediatR.ApiSpec3_0Test.Dtos;
using ApiFirstMediatR.ApiSpec3_0Test.Requests;
using MediatR;

namespace ApiFirstMediatR.ApiSpec3_0Test;

public class FindPetsQueryHandler : IRequestHandler<FindPetsQuery, List<Pet>>
{
    public Task<List<Pet>> Handle(FindPetsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}