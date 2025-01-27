using eBay.Fulfillment.API.Requests;
using eBay.Fulfillment.API.Dtos;

namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Orders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, OrderSearchPagedCollection>
{
    public Task<OrderSearchPagedCollection> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}