namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class ModelRepository<T> : IRepository<T>
{
    private readonly IApiSpecRepository _apiSpecRepository;
    private readonly IOpenApiDocumentMapper<T> _mapper;
    private readonly Lazy<List<T>> _controllers;

    public ModelRepository(IApiSpecRepository apiSpecRepository, IOpenApiDocumentMapper<T> mapper)
    {
        _apiSpecRepository = apiSpecRepository;
        _mapper = mapper;
        _controllers = new Lazy<List<T>>(LazyGet);
    }

    public IEnumerable<T> Get()
    {
        return _controllers.Value;
    }

    private List<T> LazyGet()
    {
        var apiSpec = _apiSpecRepository.Get();

        if (apiSpec is not null)
            return _mapper.Map(apiSpec).ToList();
        
        return Enumerable.Empty<T>().ToList();
    }
}