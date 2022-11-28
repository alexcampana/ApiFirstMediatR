namespace ApiFirstMediatR.Generator.Services;

internal sealed class AdditionalFileAnalysisContextWrapper : ICompilation
{
    private readonly AdditionalFileAnalysisContext _context;

    public AdditionalFileAnalysisContextWrapper(AdditionalFileAnalysisContext context)
    {
        _context = context;
    }

    public ImmutableArray<AdditionalText> AdditionalFiles => ImmutableArray.Create(_context.AdditionalFile);
    public CancellationToken CancellationToken => _context.CancellationToken;
    public Compilation Compilation => _context.Compilation;
}