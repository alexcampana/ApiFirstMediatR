using Microsoft.OpenApi.Any;

namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class DataTransferObjectMapper : IDataTransferObjectMapper, IOpenApiDocumentMapper<DataTransferObject>
{
    private readonly IPropertyMapper _propertyMapper;

    public DataTransferObjectMapper(IPropertyMapper propertyMapper)
    {
        _propertyMapper = propertyMapper;
    }

    public IEnumerable<DataTransferObject> Map(OpenApiDocument[] apiSpecs)
    {
        foreach (var apiSpec in apiSpecs)
        {
            var ns = apiSpec.Extensions.TryGetValue("x-namespace", out var documentNamespace) ? ((OpenApiString)documentNamespace).Value : "default";
            foreach (var schema in apiSpec.Components.Schemas)
            {
                var properties = _propertyMapper.Map(schema.Value, ns);

                var dto = new DataTransferObject
                {
                    Name = schema.Key.ToCleanName().ToPascalCase(),
                    Properties = properties,
                    Namespace = ns
                };

                if (schema.Value.AllOf.Any())
                {
                    var inheritedSchemas = schema
                        .Value
                        .AllOf
                        .Where(s => s.Reference?.Id is not null)
                        .ToList();

                    if (inheritedSchemas.Count() > 1)
                        throw new NotImplementedException("Only allowing one inherited schema for now");

                    dto.InheritedDto = inheritedSchemas
                        .FirstOrDefault(s => s.Reference.Id is not null)?
                        .Reference
                        .Id;

                    var allOfProperties = schema
                        .Value
                        .AllOf
                        .Where(s => s.Type == "object" && s.Reference is null)
                        .SelectMany(o => _propertyMapper.Map(o, ns));

                    dto.Properties = dto.Properties.Union(allOfProperties);
                }

                yield return dto;
            }

        }
    }
}