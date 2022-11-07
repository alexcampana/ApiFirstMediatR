namespace ApiFirstMediatR.Generator.Mappers;

internal static class PropertyMapper
{
    public static IEnumerable<Property> Map(OpenApiSchema schema)
    {
        // TODO: Add validation that this is the proper mapper for this schema
        
        foreach (var property in schema.Properties)
        {
            yield return new Property
            {
                Name = property.Key.ToPascalCase(),
                JsonName = property.Key,
                Description = property.Value.Description?.SplitOnNewLine(),
                DataType = TypeMapper.Map(property.Value),
                IsNullable = property.Value.Nullable
            };
        }
    }
}