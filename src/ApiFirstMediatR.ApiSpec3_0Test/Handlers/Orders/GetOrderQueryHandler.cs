using eBay.Fulfillment.API.Requests;
using Order = eBay.Fulfillment.API.Dtos.Order;

namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Orders;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Order>
{
    public Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}