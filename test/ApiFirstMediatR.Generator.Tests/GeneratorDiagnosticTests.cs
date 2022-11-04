namespace ApiFirstMediatR.Generator.Tests;

public class GeneratorDiagnosticTests : TestBase
{
    [Fact]
    public async Task MissingAPISpecFile_ThrowsDiagnostic()
    {
        var code = "namespace Test;";
        var inputCompilation = CreateCompilation(code);

        var generator = new ApiSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver
            .Create(generator)
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
                out var diagnostics);
        
        Assert.Single(diagnostics);
        Assert.Equal("AFM001", diagnostics.First().Id);
    }
 
    [Fact]
    public async Task EmptyAPISpecFile_ThrowsDiagnostic()
    {
        var code = "namespace Test;";
        var inputCompilation = CreateCompilation(code);
        
        var additionalTexts = new AdditionalTextYml("api_spec.yml", "") as AdditionalText;

        var generator = new ApiSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalTexts))
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
                out var diagnostics);
        
        Assert.Single(diagnostics);
        Assert.Equal("AFM002", diagnostics.First().Id);
    }
 
    [Fact]
    public async Task BadAPISpecFile_ThrowsDiagnostic()
    {
        var code = "namespace Test;";
        var inputCompilation = CreateCompilation(code);
        
        var additionalTexts = new AdditionalTextYml("api_spec.yml", @"openapi: 3.0.1
info:
  title: HelloWorld API
  version: v1
paths:
  /api/HelloWorld:
    get:
      tags:
        - HelloWorld
      operationId: GetHelloWorld
      parameters: []
      responses:
        200:") as AdditionalText;

        var generator = new ApiSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalTexts))
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
                out var diagnostics);
        
        Assert.Single(diagnostics);
        Assert.Equal("AFM003", diagnostics.First().Id);
    }
}