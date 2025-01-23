namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class ParameterMapper : IParameterMapper
{
    private readonly ITypeMapper _typeMapper;

    public ParameterMapper(ITypeMapper typeMapper)
    {
        _typeMapper = typeMapper;
    }

    public IEnumerable<Parameter> Map(IEnumerable<OpenApiParameter> openApiParameters, string? ns)
    {
        foreach (var parameter in openApiParameters)
        {
            string? dataType = null;

            if (parameter.Schema.OneOf.Any())
            {
                dataType = _typeMapper.Map(parameter.Schema.OneOf.First(), ns);
                // TODO: Throw warning diagnostic here
            }
            
            yield return new Parameter
            {
                ParameterName = parameter.Name.ToKeywordSafeName().ToCamelCase(),
                Name = parameter.Name.ToPascalCase(),
                JsonName = parameter.Name,
                Description = parameter.Description.SplitOnNewLine(),
                DataType = dataType ?? _typeMapper.Map(parameter.Schema, ns),
                IsNullable = !parameter.Required
            };
        }
    }
}