using System;
using System.Threading;
using System.Threading.Tasks;
using ApiFirstMediatR.ApiSpec2_0Test.Dtos;
using ApiFirstMediatR.ApiSpec2_0Test.Requests;
using MediatR;

namespace ApiFirstMediatR.ApiSpec2_0Test.Handlers;

public sealed class GetHistoryQueryHandler : IRequestHandler<GetHistoryQuery, Activities>
{
    public Task<Activities> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}