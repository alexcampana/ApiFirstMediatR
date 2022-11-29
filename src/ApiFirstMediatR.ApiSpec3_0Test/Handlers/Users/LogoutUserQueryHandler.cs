namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Users;

public class LogoutUserQueryHandler : IRequestHandler<LogoutUserQuery>
{
    public Task<Unit> Handle(LogoutUserQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}