namespace ApiFirstMediatR.Generator.Interfaces;

internal interface IEndpointMapper
{
    IEnumerable<Endpoint> Map(OpenApiPaths paths);
}