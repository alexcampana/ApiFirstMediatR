namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface IPropertyMapper
{
    IEnumerable<Property> Map(OpenApiSchema schema);
}