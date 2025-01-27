using eBay.Fulfillment.API.Dtos;
using eBay.Fulfillment.API.Requests;

namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Orders;

public class GetShippingFulfillmentsQueryHandler : IRequestHandler<GetShippingFulfillmentsQuery, ShippingFulfillmentPagedCollection>
{
    public Task<ShippingFulfillmentPagedCollection> Handle(GetShippingFulfillmentsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}