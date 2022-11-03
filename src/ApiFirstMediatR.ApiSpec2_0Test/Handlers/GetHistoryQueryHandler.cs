namespace ApiFirstMediatR.ApiSpec2_0Test.Handlers;

public sealed class GetHistoryQueryHandler : IRequestHandler<GetHistoryQuery, Activities>
{
    public Task<Activities> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}