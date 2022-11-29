namespace ApiFirstMediatR.Generator.Interfaces;

public interface ICompilation
{
    ImmutableArray<AdditionalText> AdditionalFiles { get; }
    CancellationToken CancellationToken { get; }
    Compilation Compilation { get; }
}