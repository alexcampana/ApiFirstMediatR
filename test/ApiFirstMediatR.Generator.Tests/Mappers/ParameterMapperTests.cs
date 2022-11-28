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
        
        Assert.NotNull(response);
        var parameter = Assert.Single(response);
        
        Assert.Equal("testName", parameter.ParameterName);
        Assert.Equal("TestName", parameter.Name);
        Assert.Equal("testName", parameter.JsonName);
        Assert.Equal("string", parameter.DataType);
        Assert.False(parameter.IsNullable);
        Assert.NotNull(parameter.Description);
        Assert.Equal("Test Description", parameter.Description.FirstOrDefault());
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
        
        Assert.NotNull(response);
        var parameter = Assert.Single(response);
        
        Assert.Equal("testName", parameter.ParameterName);
        Assert.Equal("TestName", parameter.Name);
        Assert.Equal("test_name", parameter.JsonName);
        Assert.Equal("string", parameter.DataType);
        Assert.False(parameter.IsNullable);
        Assert.NotNull(parameter.Description);
        Assert.Equal("Test Description", parameter.Description.FirstOrDefault());
    }
}