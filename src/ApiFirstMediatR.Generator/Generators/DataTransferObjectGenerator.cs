namespace ApiFirstMediatR.Generator.Generators;

internal sealed class DataTransferObjectGenerator : IApiGenerator
{
    private readonly GeneratorExecutionContext _context;
    private readonly IRepository<DataTransferObject> _dtoRepository;

    public DataTransferObjectGenerator(GeneratorExecutionContext context, IRepository<DataTransferObject> dtoRepository)
    {
        _context = context;
        _dtoRepository = dtoRepository;
    }

    public void Generate()
    {
        var projectConfig = new ScriptObject();
        projectConfig.Add("namespace", _context.Compilation.AssemblyName);

        var dtos = _dtoRepository.Get();
        
        foreach (var dto in dtos)
        {
            var sourceText = ApiTemplate.DataTransferObject.Generate(dto, projectConfig);
            _context.AddSource($"Dtos_{dto.Name}.g.cs", sourceText);
        }
    }
}