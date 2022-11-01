namespace ApiFirstMediatR.Generator.Mappers;

internal static class PropertyMapper
{
    public static IEnumerable<Property> Map(OpenApiSchema schema)
    {
        foreach (var property in schema.Properties)
        {
            yield return new Property
            {
                Name = property.Key.ToPascalCase(),
                DataType = MapType(property.Value),
                IsNullable = property.Value.Nullable
            };
        }
    }

    private static string MapType(OpenApiSchema apiType)
        => (apiType.Type, apiType.Format) switch
        {
            ("boolean", _) => "bool",
            ("integer", "int32") => "int",
            ("integer", "int64") => "long",
            ("number", "float") => "float",
            ("number", "double") => "double",
            ("number", _) => "decimal",
            ("string", "date") => "DateOnly",
            ("string", "date-time") => "DateTimeOffset",
            ("string", _) => "string",
            ("array", _) => $"List<{MapArrayItem(apiType)}>",
            (_, _) => apiType.Type
        };

    private static string MapArrayItem(OpenApiSchema apiType)
    {
        if (apiType.Items.Reference?.Id is not null)
        {
            return apiType.Items.Reference.Id;
        }

        return apiType.Items.Type;
    }
}