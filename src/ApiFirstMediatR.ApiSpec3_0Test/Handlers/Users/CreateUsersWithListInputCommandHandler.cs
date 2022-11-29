namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Users;

public class CreateUsersWithListInputCommandHandler : IRequestHandler<CreateUsersWithListInputCommand, User>
{
    public Task<User> Handle(CreateUsersWithListInputCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}