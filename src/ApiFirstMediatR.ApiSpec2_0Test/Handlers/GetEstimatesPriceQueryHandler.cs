namespace ApiFirstMediatR.ApiSpec2_0Test.Handlers;

public sealed class GetEstimatesPriceQueryHandler : IRequestHandler<GetEstimatesPriceQuery, List<PriceEstimate>>
{
    public Task<List<PriceEstimate>> Handle(GetEstimatesPriceQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}