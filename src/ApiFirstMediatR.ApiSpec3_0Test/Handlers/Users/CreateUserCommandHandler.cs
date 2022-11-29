namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    public Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}