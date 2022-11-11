namespace ApiFirstMediatR.Generator.Generators;

internal sealed class DataTransferObjectGenerator : IApiGenerator
{
    private readonly GeneratorExecutionContext _context;
    private readonly IRepository<DataTransferObject> _dtoRepository;
    private readonly IApiConfigRepository _apiConfigRepository;

    public DataTransferObjectGenerator(GeneratorExecutionContext context, IRepository<DataTransferObject> dtoRepository, IApiConfigRepository apiConfigRepository)
    {
        _context = context;
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
            _context.AddSource($"Dtos_{dto.Name}.g.cs", sourceText);
        }
    }
}