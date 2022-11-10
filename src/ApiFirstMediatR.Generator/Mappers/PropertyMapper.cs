namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class PropertyMapper : IPropertyMapper
{
    private readonly ITypeMapper _typeMapper;

    public PropertyMapper(ITypeMapper typeMapper)
    {
        _typeMapper = typeMapper;
    }

    public IEnumerable<Property> Map(OpenApiSchema schema)
    {
        // TODO: Add validation that this is the proper mapper for this schema
        
        foreach (var property in schema.Properties)
        {
            yield return new Property
            {
                Name = property.Key.ToPascalCase(),
                JsonName = property.Key,
                Description = property.Value.Description?.SplitOnNewLine(),
                DataType = _typeMapper.Map(property.Value),
                IsNullable = property.Value.Nullable || !schema.Required.Contains(property.Key) // TODO: Switch logic here based on spec version (version 3 checks nullable, version 2 checks required
            };
        }
    }
}