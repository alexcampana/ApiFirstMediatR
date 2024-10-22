namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Pets;

public sealed class DeletePetCommandHandler : IRequestHandler<DeletePetCommand>
{
    public Task Handle(DeletePetCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}