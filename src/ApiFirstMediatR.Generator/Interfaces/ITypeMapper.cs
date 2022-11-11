namespace ApiFirstMediatR.Generator.Interfaces;

internal interface ITypeMapper
{
    string Map(OpenApiSchema schema);
}