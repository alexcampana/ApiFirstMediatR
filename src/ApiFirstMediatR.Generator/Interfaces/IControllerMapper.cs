namespace ApiFirstMediatR.Generator.Interfaces;

internal interface IControllerMapper
{
    IEnumerable<Controller> Map(OpenApiDocument apiSpec);
}