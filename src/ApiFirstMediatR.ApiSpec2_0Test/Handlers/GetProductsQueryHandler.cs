namespace ApiFirstMediatR.ApiSpec2_0Test.Handlers;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
{
    public Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}