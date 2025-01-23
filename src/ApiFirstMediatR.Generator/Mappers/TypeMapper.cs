
namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class TypeMapper : ITypeMapper
{
    private readonly IApiConfigRepository _apiConfigRepository;

    public TypeMapper(IApiConfigRepository apiConfigRepository)
    {
        _apiConfigRepository = apiConfigRepository;
    }

    public string Map(OpenApiSchema schema, string? ns = null)
    {
        var apiConfig = _apiConfigRepository.Get();
        // TODO: Support non schema objects (through custom response objects)
        //       This will required parsing all responses and requests to ensure all dto
        //       are created for all endpoints
        
        // TODO: Add validation that this is the proper mapper for this schema
        var nsToUse = !string.IsNullOrWhiteSpace(ns) && ns != "default" ? ns : apiConfig.Namespace;
        if (schema.Reference is not null)
            return $"{nsToUse}.Dtos.{schema.Reference.Id.ToPascalCase()}";
        
        return (schema.Type?.ToLower(), schema.Format?.ToLower()) switch
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