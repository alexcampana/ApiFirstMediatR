namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface IResponseMapper
{
    Response Map(OpenApiResponses responses);
}