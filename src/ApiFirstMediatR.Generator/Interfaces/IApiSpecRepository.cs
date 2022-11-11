namespace ApiFirstMediatR.Generator.Interfaces;

internal interface IApiSpecRepository
{
    OpenApiDocument? Get();
}