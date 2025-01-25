namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class PropertyMapper : IPropertyMapper
{
    private readonly ITypeMapper _typeMapper;

    public PropertyMapper(ITypeMapper typeMapper)
    {
        _typeMapper = typeMapper;
    }

    public IEnumerable<Property> Map(OpenApiSchema schema, string? ns)
    {
        // TODO: Add validation that this is the proper mapper for this schema
        var referenceName = schema.Reference?.Id?.ToCleanName().ToPascalCase();
        
        foreach (var property in schema.Properties)
        {
            var name = property.Key.ToCleanName().ToPascalCase();

            // Member names can't be the same as their enclosing type, so switching to camelcase
            if (name == referenceName)
            {
                name = name.ToCleanName().ToCamelCase();
            }

            var dataType = _typeMapper.Map(property.Value, ns);

            // overriding TypeMapper for enums as it doesn't have the context of the dto
            if (property.Value.Enum.Any())
            {
                dataType = $"{referenceName}{property.Key.ToPascalCase()}";// TODO: Refactor to use the same logic as the DataTransferObjectEnumMapper 
            }

            yield return new Property
            {
                Name = name,
                JsonName = property.Key,
                Description = property.Value.Description?.SplitOnNewLine(),
                DataType = dataType,
                IsNullable = property.Value.Nullable || !schema.Required.Contains(property.Key) // TODO: Switch logic here based on spec version (version 3 checks nullable, version 2 checks required
            };
        }
    }
}