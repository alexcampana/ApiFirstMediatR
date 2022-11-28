using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<ApiFirstMediatR.Generator.ApiAnalyzer>;

namespace ApiFirstMediatR.Generator.Tests.Analyzers;

public class ApiMissingImplementationTests
{
    [Fact]
    public async Task MissingImplementation_ThrowsDiagnostic()
    {
        await new CSharpAnalyzerTest<ApiAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources = { "" },
                ExpectedDiagnostics =
                {
                    Verify
                        .Diagnostic(DiagnosticIdentifiers.ApiMissingImplementation)
                        .WithLocation("api_spec.yml", 1, 1)
                        .WithMessage(
                            "Missing MediatR Handler Implementation: GetHelloWorldQueryHandler : IRequestHandler<GetHelloWorldQuery, TestProject.Dtos.HelloWorldDto>")
                },
                AdditionalFiles =
                {
                    ("api_spec.yml", ApiSpec)
                }
            }
        }.RunAsync();
    }
    
    // TODO: Add happy path test where implementation is provided.
    //       Since this requires the generator and other dependencies,
    //       setup is difficult and instead currently relying on existing
    //       test projects in repo to validate happy path

private const string ApiSpec = @"openapi: 3.0.1
info:
  title: HelloWorld API
  version: v1
paths:
  /api/HelloWorld/{helloWorldId}:
    get:
      tags:
        - HelloWorld
      responses:
        '200':
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/HelloWorldDto'
          description: Hello world!
      operationId: GetHelloWorld
      description: Gets a HelloWorld Message
      parameters:
        - in: path
          name: helloWorldId
          required: true
          schema:
            type: integer
            format: int32
components:
  schemas:
    HelloWorldDto:
      type: object
      properties:
        message:
          nullable: true
          type: string
security: []";
}