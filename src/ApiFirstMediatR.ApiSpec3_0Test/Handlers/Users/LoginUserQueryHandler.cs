namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Users;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
{
    public Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}