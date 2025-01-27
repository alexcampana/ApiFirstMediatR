using eBay.Fulfillment.API.Dtos;
using eBay.Fulfillment.API.Requests;

namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Orders;

public class IssueRefundCommandHandler : IRequestHandler<IssueRefundCommand, Refund>
{
    public Task<Refund> Handle(IssueRefundCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}