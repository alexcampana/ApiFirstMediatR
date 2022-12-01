namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface IDataTransferObjectMapper
{
    IEnumerable<DataTransferObject> Map(OpenApiDocument apiSpec);
}