namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class DataTransferObjectMapperTests
{
    private readonly IDataTransferObjectMapper _dataTransferObjectMapper;

    public DataTransferObjectMapperTests()
    {
        var typeMapper = new TypeMapper(MockApiConfig.Create());
        var propertyMapper = new PropertyMapper(typeMapper);
        _dataTransferObjectMapper = new DataTransferObjectMapper(propertyMapper);
    }

    [Fact]
    public void ValidSchema_HappyPath()
    {
        var apiSpec = new OpenApiDocument
        {
            Components = new OpenApiComponents
            {
                Schemas = new Dictionary<string, OpenApiSchema>
                {
                    ["Test"] = new OpenApiSchema
                    {
                        Type = "Object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["id"] = new OpenApiSchema
                            {
                                Type = "integer",
                                Format = "int32"
                            },
                            ["name"] = new OpenApiSchema
                            {
                                Type = "string"
                            }
                        }
                    }
                }
            }
        };

        var dtos = _dataTransferObjectMapper.Map(apiSpec);
        dtos.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                Name = "Test",
                Properties = new[]
                {
                    new
                    {
                        Name = "Id",
                        JsonName = "id",
                        DataType = "int"
                    },
                    new
                    {
                        Name = "Name",
                        JsonName = "name",
                        DataType = "string"
                    }
                }
            });
    }

    [Fact]
    public void ValidSchema_InheritedSchema_HappyPath()
    {
        var apiSpec = new OpenApiDocument
        {
            Components = new OpenApiComponents
            {
                Schemas = new Dictionary<string, OpenApiSchema>
                {
                    ["Test"] = new OpenApiSchema
                    {
                        Type = "Object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["id"] = new OpenApiSchema
                            {
                                Type = "integer",
                                Format = "int32"
                            },
                            ["name"] = new OpenApiSchema
                            {
                                Type = "string"
                            }
                        }
                    },
                    ["SubTest"] = new OpenApiSchema
                    {
                        Type = "Object",
                        AllOf = new []
                        {
                            new OpenApiSchema
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Test",
                                    Type = ReferenceType.Schema
                                }
                            }
                        },
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["test"] = new OpenApiSchema
                            {
                                Type = "integer",
                                Format = "int64"
                            }
                        }
                    }
                }
            }
        };

        var dtos = _dataTransferObjectMapper.Map(apiSpec);
        dtos.Should().HaveCount(2)
            .And.ContainEquivalentOf(new
            {
                Name = "Test",
                Properties = new[]
                {
                    new
                    {
                        Name = "Id",
                        JsonName = "id",
                        DataType = "int"
                    },
                    new
                    {
                        Name = "Name",
                        JsonName = "name",
                        DataType = "string"
                    }
                }
            })
            .And.ContainEquivalentOf(new
            {
                Name = "SubTest",
                InheritedDto = "Test",
                Properties = new []
                {
                    new
                    {
                        Name = "Test",
                        JsonName = "test",
                        DataType = "long"
                    }
                }
            });
    }
}