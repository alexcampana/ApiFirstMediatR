namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Store;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    public Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}