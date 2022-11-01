namespace ApiFirstMediatR.Generator.Mappers;

internal static class DataTransferObjectMapper
{
    public static IEnumerable<DataTransferObject> Map(OpenApiDocument apiSpec)
    {
        foreach (var schema in apiSpec.Components.Schemas)
        {
            var properties = PropertyMapper.Map(schema.Value);

            var dto = new DataTransferObject
            {
                Name = schema.Key,
                Properties = properties
            };

            if (schema.Value.AllOf.Any())
            {
                var inheritedSchemas = schema
                    .Value
                    .AllOf
                    .Where(s => s.Reference?.Id is not null);

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
                    .SelectMany(s => PropertyMapper.Map(s));

                dto.Properties = dto.Properties.Union(allOfProperties);
            }

            yield return dto;
        }
    }
}