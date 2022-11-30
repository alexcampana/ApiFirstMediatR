using Microsoft.OpenApi.Any;

namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class DataTransferObjectEnumMapper : IDataTransferObjectEnumMapper, IOpenApiDocumentMapper<DataTransferObjectEnum>
{
    public IEnumerable<DataTransferObjectEnum> Map(OpenApiDocument apiSpec)
    {
        foreach (var schema in apiSpec.Components.Schemas)
        {
            foreach (var property in schema.Value.Properties)
            {
                if (property.Value.Enum.Any())
                {
                    var enumValues = property.Value.Enum
                        .OfType<OpenApiString>()
                        .Select(e => new DataTransferObjectEnumValue
                        {
                            Name = e.Value.ToCleanName().ToPascalCase(),
                            JsonName = e.Value
                        });

                    yield return new DataTransferObjectEnum
                    {
                        Name = $"{schema.Key.ToCleanName().ToPascalCase()}{property.Key.ToPascalCase()}", // TODO: Make this configurable
                        EnumValues = enumValues
                    };
                }
            }
        }
    }
}