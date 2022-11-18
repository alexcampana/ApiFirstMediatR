namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class TypeMapperTests
{
    private readonly ITypeMapper _typeMapper;
    
    public TypeMapperTests()
    {
        var mockApiConfigRepo = new Mock<IApiConfigRepository>();
        mockApiConfigRepo
            .Setup(mock => mock.Get())
            .Returns(new ApiConfig
            {
                Namespace = "Test"
            });
        
        _typeMapper = new TypeMapper(mockApiConfigRepo.Object);
    }

    [Fact]
    public void ValidType_WithNativeType_HappyPath()
    {
        var schema = new OpenApiSchema
        {
            Type = "integer"
        };

        var type = _typeMapper.Map(schema);
        Assert.Equal("int", type);
    }

    [Fact]
    public void ValidType_WithTypePassthrough_HappyPath()
    {
        var schema = new OpenApiSchema
        {
            Type = "CustomType"
        };

        var type = _typeMapper.Map(schema);
        Assert.Equal("CustomType", type);
    }

    [Fact]
    public void ValidType_NothingDefined_HappyPath()
    {
        var schema = new OpenApiSchema();
        var type = _typeMapper.Map(schema);
        Assert.Equal("object", type);
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

        var type = _typeMapper.Map(schema);
        Assert.Equal("Test.Dtos.TestDto", type);
    }
}