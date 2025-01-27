using eBay.Fulfillment.API.Dtos;
using eBay.Fulfillment.API.Requests;

namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Orders;

public class GetShippingFulfillmentQueryHandler : IRequestHandler<GetShippingFulfillmentQuery, ShippingFulfillment>
{
    public Task<ShippingFulfillment> Handle(GetShippingFulfillmentQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}