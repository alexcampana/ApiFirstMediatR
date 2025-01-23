namespace ApiFirstMediatR.Generator.Tests.SourceGenerators;

public class ApiSpecDiagnosticTests : TestBase
{
    [Fact]
    public void MissingAPISpecFile_ThrowsDiagnostic()
    {
        var code = "namespace Test;";
        var inputCompilation = CreateCompilation(code);

        var generator = new SourceGenerator();
        CSharpGeneratorDriver
            .Create(generator)
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
                out var diagnostics);

        diagnostics.Should().Contain(o => o.Id == DiagnosticIdentifiers.ApiSpecFileNotFound);
    }
 
    [Fact]
    public void EmptyAPISpecFile_ThrowsDiagnostic()
    {
        var result = RunGenerators("Test", "");

        result.Diagnostics.Should().Contain(o => o.Id == DiagnosticIdentifiers.ApiSpecFileEmpty);
    }
 
    [Fact]
    public void BadAPISpecFile_ThrowsDiagnostic()
    {
        var result = RunGenerators("Test", BadApiSpec);
        result.Diagnostics.Should().Contain(o => o.Id == DiagnosticIdentifiers.ApiSpecFileParsingError);
    }
    
    private const string BadApiSpec = @"openapi: 3.0.1
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
        200:";
}