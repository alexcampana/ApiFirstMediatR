using System.Collections.Immutable;

namespace ApiFirstMediatR.Generator.Interfaces;

internal interface ISources
{
    void AddSource(string hintName, string source);
    ImmutableArray<AdditionalText> AdditionalFiles { get; }
    CancellationToken CancellationToken { get; }
    Compilation Compilation { get; }
}