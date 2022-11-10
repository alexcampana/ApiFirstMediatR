namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class ParameterMapper : IParameterMapper
{
    private readonly ITypeMapper _typeMapper;

    public ParameterMapper(ITypeMapper typeMapper)
    {
        _typeMapper = typeMapper;
    }

    public IEnumerable<Parameter> Map(IEnumerable<OpenApiParameter> openApiParameters)
    {
        foreach (var parameter in openApiParameters)
        {
            yield return new Parameter
            {
                ParameterName = parameter.Name.ToCamelCase(),
                Name = parameter.Name.ToPascalCase(),
                JsonName = parameter.Name,
                Description = parameter.Description.SplitOnNewLine(),
                DataType = _typeMapper.Map(parameter.Schema),
                IsNullable = !parameter.Required
            };
        }
    }
}