namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers;

public sealed class DeletePetCommandHandler : IRequestHandler<DeletePetCommand, Unit>
{
    public Task<Unit> Handle(DeletePetCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Unit.Value);
    }
}