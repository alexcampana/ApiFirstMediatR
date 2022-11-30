namespace ApiFirstMediatR.Generator.Interfaces.Repositories;

internal interface IApiSpecRepository
{
    OpenApiDocument? Get();
}