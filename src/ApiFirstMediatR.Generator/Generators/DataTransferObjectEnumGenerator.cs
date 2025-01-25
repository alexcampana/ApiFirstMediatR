namespace ApiFirstMediatR.Generator.Generators;

internal sealed class DataTransferObjectEnumGenerator : IApiGenerator
{
    private readonly ISources _sources;
    private readonly IRepository<DataTransferObjectEnum> _dtoEnumRepository;
    private readonly IApiConfigRepository _apiConfigRepository;

    public DataTransferObjectEnumGenerator(ISources sources, IRepository<DataTransferObjectEnum> dtoEnumRepository, IApiConfigRepository apiConfigRepository)
    {
        _sources = sources;
        _dtoEnumRepository = dtoEnumRepository;
        _apiConfigRepository = apiConfigRepository;
    }

    public void Generate()
    {
        var projectConfig = _apiConfigRepository.Get();
        var dtoEnums = _dtoEnumRepository.Get();

        foreach (var dtoEnum in dtoEnums)
        {
            var baseNamespace = projectConfig.Namespace;
            if (!string.IsNullOrWhiteSpace(dtoEnum.Namespace) && dtoEnum.Namespace != "default")
            {
                projectConfig.Namespace = dtoEnum.Namespace;
            }
            var sourceText = ApiTemplate.DataTransferObjectEnum.Generate(dtoEnum, projectConfig);
            _sources.AddSource($"{dtoEnum.Namespace}/Dtos_{dtoEnum.Name}.g.cs", sourceText);
            projectConfig.Namespace = baseNamespace;
        }
    }
}