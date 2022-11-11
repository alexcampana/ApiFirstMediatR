namespace ApiFirstMediatR.Generator.Interfaces;

internal interface IDataTransferObjectMapper
{
    IEnumerable<DataTransferObject> Map(OpenApiDocument apiSpec);
}