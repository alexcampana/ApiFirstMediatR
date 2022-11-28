namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class PropertyMapperTests
{
    private readonly IPropertyMapper _propertyMapper;

    public PropertyMapperTests()
    {
        var mockApiConfigRepo = MockApiConfig.Create();
        var typeMapper = new TypeMapper(mockApiConfigRepo);
        
        _propertyMapper = new PropertyMapper(typeMapper);
    }

    [Fact]
    public void ValidProperty_HappyPath()
    {
        var schema = new OpenApiSchema
        {
            Reference = new OpenApiReference
            {
                Id = "TestSchema"
            },
            Properties =
            {
                ["TestPropertyOne"] = new OpenApiSchema
                {
                    Type = "integer",
                    Nullable = true,
                    Description = "Test Description"
                },
                ["TestPropertyTwo"] = new OpenApiSchema
                {
                    Type = "string"
                }
            }
        };

        var response = _propertyMapper.Map(schema);
        
        Assert.NotNull(response);
        
        var properties = response.ToList();
        Assert.Equal(2, properties.Count());
        
        var firstProperty = properties.First();
        Assert.Equal("TestPropertyOne", firstProperty.Name);
        Assert.Equal("TestPropertyOne", firstProperty.JsonName);
        Assert.NotNull(firstProperty.Description);
        Assert.Equal("Test Description", firstProperty.Description.FirstOrDefault());
        Assert.Equal("int", firstProperty.DataType);
        Assert.True(firstProperty.IsNullable);
    }

    [Fact]
    public void ValidProperty_WithSnakeCasing_HappyPath()
    {
        var schema = new OpenApiSchema
        {
            Reference = new OpenApiReference
            {
                Id = "test_schema"
            },
            Properties =
            {
                ["test_property_one"] = new OpenApiSchema
                {
                    Type = "integer",
                    Description = "Test Description"
                },
                ["test_property_two"] = new OpenApiSchema
                {
                    Type = "string"
                }
            },
            Required =
            {
                "test_property_one"
            }
        };

        var response = _propertyMapper.Map(schema);
        
        Assert.NotNull(response);
        
        var properties = response.ToList();
        Assert.Equal(2, properties.Count());
        
        var firstProperty = properties.First();
        Assert.Equal("TestPropertyOne", firstProperty.Name);
        Assert.Equal("test_property_one", firstProperty.JsonName);
        Assert.NotNull(firstProperty.Description);
        Assert.Equal("Test Description", firstProperty.Description.FirstOrDefault());
        Assert.Equal("int", firstProperty.DataType);
        Assert.False(firstProperty.IsNullable);
    }

    [Fact]
    public void ValidProperty_WithRequiredField_HappyPath()
    {
        var schema = new OpenApiSchema
        {
            Reference = new OpenApiReference
            {
                Id = "TestSchema"
            },
            Properties =
            {
                ["TestPropertyOne"] = new OpenApiSchema
                {
                    Type = "integer",
                    Description = "Test Description"
                },
                ["TestPropertyTwo"] = new OpenApiSchema
                {
                    Type = "string"
                }
            },
            Required =
            {
                "TestPropertyOne"
            }
        };

        var response = _propertyMapper.Map(schema);
        
        Assert.NotNull(response);
        
        var properties = response.ToList();
        Assert.Equal(2, properties.Count());
        
        var firstProperty = properties.First();
        Assert.Equal("TestPropertyOne", firstProperty.Name);
        Assert.Equal("TestPropertyOne", firstProperty.JsonName);
        Assert.NotNull(firstProperty.Description);
        Assert.Equal("Test Description", firstProperty.Description.FirstOrDefault());
        Assert.Equal("int", firstProperty.DataType);
        Assert.False(firstProperty.IsNullable);

        var secondProperty = properties[1];
        Assert.True(secondProperty.IsNullable);
    }
}