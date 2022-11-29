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

        response.Should().NotBeNull()
            .And.HaveCount(2)
            .And.ContainEquivalentOf(new
            {
                Name = "TestPropertyOne",
                JsonName = "TestPropertyOne",
                Description = new[]
                {
                    "Test Description"
                },
                DataType = "int",
                IsNullable = true
            });
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

        response.Should().NotBeNull()
            .And.HaveCount(2)
            .And.ContainEquivalentOf(new
            {
                Name = "TestPropertyOne",
                JsonName = "test_property_one",
                Description = new[]
                {
                    "Test Description"
                },
                DataType = "int",
                IsNullable = false
            });
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

        response.Should().NotBeNull()
            .And.HaveCount(2)
            .And.ContainEquivalentOf(
                new
                {
                    Name = "TestPropertyOne",
                    JsonName = "TestPropertyOne",
                    Description = new[]
                    {
                        "Test Description"
                    },
                    DataType = "int",
                    IsNullable = false
                })
            .And.ContainEquivalentOf(
                new
                {
                    Name = "TestPropertyTwo",
                    JsonName = "TestPropertyTwo",
                    DataType = "string",
                    IsNullable = true
                }
            );
    }
}