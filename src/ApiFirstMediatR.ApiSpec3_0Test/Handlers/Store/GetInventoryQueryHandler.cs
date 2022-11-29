namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Store;

public class GetInventoryQueryHandler : IRequestHandler<GetInventoryQuery, object>
{
    public Task<object> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}