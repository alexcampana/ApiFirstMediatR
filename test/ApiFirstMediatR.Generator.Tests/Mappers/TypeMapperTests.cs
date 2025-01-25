namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class TypeMapperTests
{
    private readonly ITypeMapper _typeMapper;
    
    public TypeMapperTests()
    {
        var mockApiConfigRepo = MockApiConfig.Create();
        
        _typeMapper = new TypeMapper(mockApiConfigRepo);
    }

    [Fact]
    public void ValidType_WithNativeType_HappyPath()
    {
        var schema = new OpenApiSchema
        {
            Type = "integer"
        };

        var type = _typeMapper.Map(schema, "default");
        type.Should().Be("int");
    }

    [Fact]
    public void ValidType_WithTypePassthrough_HappyPath()
    {
        var schema = new OpenApiSchema
        {
            Type = "CustomType"
        };

        var type = _typeMapper.Map(schema, "default");
        type.Should().Be("CustomType");
    }

    [Fact]
    public void ValidType_NothingDefined_HappyPath()
    {
        var schema = new OpenApiSchema();
        var type = _typeMapper.Map(schema, "default");
        type.Should().Be("object");
    }

    [Fact]
    public void ValidType_ReferencedSchema_HappyPath()
    {
        var schema = new OpenApiSchema
        {
            Reference = new OpenApiReference
            {
                Id = "TestDto"
            }
        };

        var type = _typeMapper.Map(schema, "default");
        type.Should().Be("Test.Dtos.TestDto");
    }
}