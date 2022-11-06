namespace ApiFirstMediatR.Generator.Mappers;

internal static class ParameterMapper
{
    public static IEnumerable<Parameter> Map(IEnumerable<OpenApiParameter> openApiParameters)
    {
        foreach (var parameter in openApiParameters)
        {
            yield return new Parameter
            {
                ParameterName = parameter.Name.ToCamelCase(),
                Name = parameter.Name.ToPascalCase(),
                JsonName = parameter.Name,
                Description = parameter.Description.SplitOnNewLine(),
                DataType = TypeMapper.Map(parameter.Schema),
                IsNullable = !parameter.Required
            };
        }
    }
}