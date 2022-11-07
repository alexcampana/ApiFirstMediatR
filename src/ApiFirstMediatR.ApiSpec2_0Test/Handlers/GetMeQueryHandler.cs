using System;
using System.Threading;
using System.Threading.Tasks;
using ApiFirstMediatR.ApiSpec2_0Test.Dtos;
using ApiFirstMediatR.ApiSpec2_0Test.Requests;
using MediatR;

namespace ApiFirstMediatR.ApiSpec2_0Test.Handlers;

public sealed class GetMeQueryHandler : IRequestHandler<GetMeQuery, Profile>
{
    public Task<Profile> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}