namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Store;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
{
    public Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}