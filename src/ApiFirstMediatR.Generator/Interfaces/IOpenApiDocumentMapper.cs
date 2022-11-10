namespace ApiFirstMediatR.Generator.Interfaces;

public interface IOpenApiDocumentMapper<out T>
{
    IEnumerable<T> Map(OpenApiDocument apiSpec);
}