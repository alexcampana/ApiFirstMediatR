
namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class TypeMapper : ITypeMapper
{
    private readonly IApiConfigRepository _apiConfigRepository;

    public TypeMapper(IApiConfigRepository apiConfigRepository)
    {
        _apiConfigRepository = apiConfigRepository;
    }

    public string Map(OpenApiSchema schema)
    {
        var apiConfig = _apiConfigRepository.Get();
        
        // TODO: Add validation that this is the proper mapper for this schema
        if (schema.Reference is not null)
            return $"{apiConfig.DtoNamespace}.{schema.Reference.Id.ToPascalCase()}";
        
        return (schema.Type, schema.Format) switch
        {
            ("boolean", _) => "bool",
            ("integer", "int64") => "long",
            ("integer", _) => "int",
            ("number", "float") => "float",
            ("number", "double") => "double",
            ("number", _) => "decimal",
            ("string", "date") => "System.DateOnly",
            ("string", "date-time") => "System.DateTimeOffset",
            ("string", _) => "string",
            ("array", _) => $"System.Collections.Generic.IEnumerable<{Map(schema.Items)}>",
            (_, _) => schema.Type ?? "object" // TODO: Add support for multiple response types
        };
    }
}