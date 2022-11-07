using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApiFirstMediatR.ApiSpec2_0Test.Dtos;
using ApiFirstMediatR.ApiSpec2_0Test.Requests;
using MediatR;

namespace ApiFirstMediatR.ApiSpec2_0Test.Handlers;

public sealed class GetEstimatesPriceQueryHandler : IRequestHandler<GetEstimatesPriceQuery, IEnumerable<PriceEstimate>>
{
    public Task<IEnumerable<PriceEstimate>> Handle(GetEstimatesPriceQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}