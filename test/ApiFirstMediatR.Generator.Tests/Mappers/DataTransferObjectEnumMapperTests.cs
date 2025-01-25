using Microsoft.OpenApi.Any;

namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class DataTransferObjectEnumMapperTests
{
    private readonly IDataTransferObjectEnumMapper _dtoEnumMapper;

    public DataTransferObjectEnumMapperTests()
    {
        _dtoEnumMapper = new DataTransferObjectEnumMapper();
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
                            ["status"] = new OpenApiSchema
                            {
                                Type = "string",
                                Enum = new IOpenApiAny[]
                                {
                                    new OpenApiString("placed"),
                                    new OpenApiString("approved"),
                                    new OpenApiString("delivered")
                                }
                            }
                        }
                    }
                }
            }
        };

        var enums = _dtoEnumMapper.Map(new[] { apiSpec });
        enums.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                Name = "TestStatus",
                EnumValues = new[]
                {
                    new
                    {
                        Name = "Placed",
                        JsonName = "placed"
                    },
                    new
                    {
                        Name = "Approved",
                        JsonName = "approved"
                    },
                    new
                    {
                        Name = "Delivered",
                        JsonName = "delivered"
                    }
                }
            });
    }
}