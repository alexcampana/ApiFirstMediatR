namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

public interface IOpenApiDocumentMapper<out T>
{
    IEnumerable<T> Map(OpenApiDocument[] apiSpecs);
}