namespace ApiFirstMediatR.Generator.Interfaces;

internal interface IParameterMapper
{
    IEnumerable<Parameter> Map(IEnumerable<OpenApiParameter> openApiParameters);
}