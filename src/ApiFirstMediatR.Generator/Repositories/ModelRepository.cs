namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class ModelRepository<T> : IRepository<T>
{
    private readonly IApiSpecRepository _apiSpecRepository;
    private readonly IOpenApiDocumentMapper<T> _mapper;
    private readonly object _lock = new ();
    private IEnumerable<T>? _controllers;

    public ModelRepository(IApiSpecRepository apiSpecRepository, IOpenApiDocumentMapper<T> mapper)
    {
        _apiSpecRepository = apiSpecRepository;
        _mapper = mapper;
    }

    public IEnumerable<T> Get()
    {
        lock (_lock)
        {
            if (_controllers is null)
            {
                var apiSpec = _apiSpecRepository.Get();

                if (apiSpec is not null)
                    _controllers = _mapper.Map(apiSpec);
                else
                    _controllers = Enumerable.Empty<T>();
            }
        }

        return _controllers;
    }
}