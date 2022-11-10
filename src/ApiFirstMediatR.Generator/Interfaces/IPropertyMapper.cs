namespace ApiFirstMediatR.Generator.Interfaces;

internal interface IPropertyMapper
{
    IEnumerable<Property> Map(OpenApiSchema schema);
}