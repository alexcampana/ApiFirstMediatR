namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface IDataTransferObjectEnumMapper
{
    IEnumerable<DataTransferObjectEnum> Map(OpenApiDocument[] apiSpec);
}