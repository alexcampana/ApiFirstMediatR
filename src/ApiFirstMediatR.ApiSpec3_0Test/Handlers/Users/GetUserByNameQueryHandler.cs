namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Users;

public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, ApiFirstMediatR.ApiSpec3_0Test.Dtos.User>
{
    public Task<User> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}