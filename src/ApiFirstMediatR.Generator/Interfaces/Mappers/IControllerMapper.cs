namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface IControllerMapper
{
    IEnumerable<Controller> Map(OpenApiDocument[] apiSpec);
}