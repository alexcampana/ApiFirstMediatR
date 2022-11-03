namespace ApiFirstMediatR.ApiSpec2_0Test.Handlers;

public sealed class GetMeQueryHandler : IRequestHandler<GetMeQuery, Profile>
{
    public Task<Profile> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}