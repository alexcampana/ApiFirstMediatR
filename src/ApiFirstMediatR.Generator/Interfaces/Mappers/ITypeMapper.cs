namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface ITypeMapper
{
    string Map(OpenApiSchema schema);
}