namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Store;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, OrderPlaced>
{
    public Task<OrderPlaced> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new OrderPlaced
        {
            Id = 1
        });
    }
}