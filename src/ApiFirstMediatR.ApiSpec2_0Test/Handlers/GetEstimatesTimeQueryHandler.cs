using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApiFirstMediatR.ApiSpec2_0Test.Dtos;
using ApiFirstMediatR.ApiSpec2_0Test.Requests;
using MediatR;

namespace ApiFirstMediatR.ApiSpec2_0Test.Handlers;

public sealed class GetEstimatesTimeQueryHandler : IRequestHandler<GetEstimatesTimeQuery, IEnumerable<Product>>
{
    public Task<IEnumerable<Product>> Handle(GetEstimatesTimeQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}