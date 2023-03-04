using System.Security.Cryptography;

namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class ParameterMapperTests
{
    private readonly IParameterMapper _parameterMapper;

    public ParameterMapperTests()
    {
        var mockApiConfigRepo = MockApiConfig.Create();
        var typeMapper = new TypeMapper(mockApiConfigRepo);

        _parameterMapper = new ParameterMapper(typeMapper);
    }

    [Fact]
    public void ValidParameter_HappyPath()
    {
        var openApiParameters = new List<OpenApiParameter>
        {
            new OpenApiParameter
            {
                Name = "testName",
                Description = "Test Description",
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Required = true
            }
        };

        var response = _parameterMapper.Map(openApiParameters);

        response.Should().NotBeNull()
            .And.ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                ParameterName = "testName",
                Name = "TestName",
                JsonName = "testName",
                DataType = "string",
                IsNullable = false,
                Description = new[]
                {
                    "Test Description"
                }
            });
    }

    [Fact]
    public void ValidParameter_WithSnakeCase_HappyPath()
    {
        var openApiParameters = new List<OpenApiParameter>
        {
            new OpenApiParameter
            {
                Name = "test_name",
                Description = "Test Description",
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Required = true
            }
        };

        var response = _parameterMapper.Map(openApiParameters);

        response.Should().NotBeNull()
            .And.ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                ParameterName = "testName",
                Name = "TestName",
                JsonName = "test_name",
                DataType = "string",
                IsNullable = false,
                Description = new[]
                {
                    "Test Description"
                }
            });
    }

    [Fact]
    public void NoParameters_EmptyCollectionReturned()
    {
        var openApiParameters = Enumerable.Empty<OpenApiParameter>();
        
        var response = _parameterMapper.Map(openApiParameters);

        response.Should().NotBeNull()
            .And.BeEmpty();
    }
}