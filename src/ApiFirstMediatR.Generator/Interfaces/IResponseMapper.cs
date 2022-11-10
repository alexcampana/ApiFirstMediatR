namespace ApiFirstMediatR.Generator.Interfaces;

internal interface IResponseMapper
{
    Response Map(OpenApiResponses responses);
}