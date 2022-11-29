namespace ApiFirstMediatR.Generator.Generators;

internal sealed class DataTransferObjectGenerator : IApiGenerator
{
    private readonly ISources _sources;
    private readonly IRepository<DataTransferObject> _dtoRepository;
    private readonly IApiConfigRepository _apiConfigRepository;

    public DataTransferObjectGenerator(ISources sources, IRepository<DataTransferObject> dtoRepository, IApiConfigRepository apiConfigRepository)
    {
        _sources = sources;
        _dtoRepository = dtoRepository;
        _apiConfigRepository = apiConfigRepository;
    }

    public void Generate()
    {
        var projectConfig = _apiConfigRepository.Get();
        var dtos = _dtoRepository.Get();
        
        foreach (var dto in dtos)
        {
            var sourceText = ApiTemplate.DataTransferObject.Generate(dto, projectConfig);
            _sources.AddSource($"Dtos_{dto.Name}.g.cs", sourceText);
        }
    }
}