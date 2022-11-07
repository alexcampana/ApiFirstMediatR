namespace ApiFirstMediatR.Generator.Mappers;

internal static class TypeMapper
{
    public static string Map(OpenApiSchema schema)
    {
        // TODO: Add validation that this is the proper mapper for this schema
        if (schema.Reference is not null)
            return schema.Reference.Id;
        
        return  (schema.Type, schema.Format) switch
        {
            ("boolean", _) => "bool",
            ("integer", "int32") => "int",
            ("integer", "int64") => "long",
            ("number", "float") => "float",
            ("number", "double") => "double",
            ("number", _) => "decimal",
            ("string", "date") => "System.DateOnly",
            ("string", "date-time") => "System.DateTimeOffset",
            ("string", _) => "string",
            ("array", _) => $"System.Collections.Generic.IEnumerable<{MapArrayItem(schema)}>",
            (_, _) => schema.Type
        };
    }

    private static string MapArrayItem(OpenApiSchema apiType)
    {
        if (apiType.Items.Reference?.Id is not null)
        {
            return apiType.Items.Reference.Id;
        }

        return apiType.Items.Type;
    }
}