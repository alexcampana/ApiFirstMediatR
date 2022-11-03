namespace ApiFirstMediatR.ApiSpec2_0Test.Handlers;

public sealed class GetEstimatesTimeQueryHandler : IRequestHandler<GetEstimatesTimeQuery, List<Product>>
{
    public Task<List<Product>> Handle(GetEstimatesTimeQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}