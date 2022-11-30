namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface IParameterMapper
{
    IEnumerable<Parameter> Map(IEnumerable<OpenApiParameter> openApiParameters);
}