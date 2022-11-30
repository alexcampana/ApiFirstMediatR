namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface IEndpointMapper
{
    IEnumerable<Endpoint> Map(OpenApiPaths paths);
}