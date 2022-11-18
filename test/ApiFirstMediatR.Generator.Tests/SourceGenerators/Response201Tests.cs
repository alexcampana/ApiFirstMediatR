namespace ApiFirstMediatR.Generator.Tests.SourceGenerators;

public class Response201Tests : TestBase
{
    [Fact]
    public void ValidAPISpec_With201Response_GeneratesValidCode()
    {
        var inputCompilation = CreateCompilation("With201Response", "");
        var additionalText = new AdditionalTextYml("api_spec.yml", ApiSpec) as AdditionalText;
        var generator = new SourceGenerator();
        var driver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalText))
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
        
        Assert.Empty(diagnostics);

        var runResult = driver.GetRunResult();
        Assert.Single(runResult.Results);

        var generatedController = runResult
            .Results
            .First()
            .GeneratedSources
            .Single(s => s.HintName == "Controllers_ApiController.g.cs");

        var controllerExpectedResult = CSharpSyntaxTree.ParseText(ExpectedController);
        Assert.True(controllerExpectedResult.IsEquivalentTo(generatedController.SyntaxTree));
    }

    [Fact]
    public void ValidAPISpec_WithUnsupportedLink_ThrowsDiagnostic()
    {
        var inputCompilation = CreateCompilation("With201Response", "");
        var additionalText = new AdditionalTextYml("api_spec.yml", ApiSpecInvalidLink) as AdditionalText;
        var generator = new SourceGenerator();
        CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalText))
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

        Assert.Single(diagnostics);
        Assert.Equal(DiagnosticIdentifiers.ApiSpecFeatureNotSupported, diagnostics.First().Id);
    }

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
  /api/HelloWorld:
    post:
      description: Posts a HelloWorld Message
      operationId: PostHelloWorld
      requestBody:
        description: HelloWorld Post Response
        required: false
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/HelloWorldDto'
      responses:
        '201':
          description: POST HelloWorld Response
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/HelloWorldCreated'
          links:
            Created HelloWorld:
              description: New HelloWorld Message
              operationId: GetHelloWorld
              parameters:
                helloWorldId: '$response.body#/id'
components:
  schemas:
    HelloWorldDto:
      type: object
      properties:
        message:
          nullable: true
          type: string
    HelloWorldCreated:
      type: object
      properties:
        id:
          type: integer
          format: int32
          description: ID of the created Hello World.
security: []";
    
    

    private const string ApiSpecInvalidLink = @"openapi: 3.0.1
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
  /api/HelloWorld:
    post:
      description: Posts a HelloWorld Message
      operationId: PostHelloWorld
      requestBody:
        description: HelloWorld Post Response
        required: false
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/HelloWorldDto'
      responses:
        '201':
          description: POST HelloWorld Response
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/HelloWorldCreated'
          links:
            Created HelloWorld:
              description: New HelloWorld Message
              operationRef: '#/paths/~1api~1HelloWorld~1{helloWorldId}/get'
              parameters:
                helloWorldId: '$response.body#/id'
components:
  schemas:
    HelloWorldDto:
      type: object
      properties:
        message:
          nullable: true
          type: string
    HelloWorldCreated:
      type: object
      properties:
        id:
          type: integer
          format: int32
          description: ID of the created Hello World.
security: []";

    private const string ExpectedController = @"// <auto-generated/>
#nullable enable

namespace With201Response.Controllers
{
    public sealed class ApiController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly MediatR.IMediator _mediator;

        public ApiController(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Gets a HelloWorld Message
        /// </summary>
        /// <param name=""helloWorldId""></param>
        /// <returns>Hello world!</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet(""/api/HelloWorld/{helloWorldId}"")]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(typeof(With201Response.Dtos.HelloWorldDto), Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<With201Response.Dtos.HelloWorldDto>> GetHelloWorld(int helloWorldId, System.Threading.CancellationToken cancellationToken)
        {
            var request = new With201Response.Requests.GetHelloWorldQuery(helloWorldId);
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        
        /// <summary>
        /// Posts a HelloWorld Message
        /// </summary>
        /// <param name=""body"">HelloWorld Post Response</param>
        /// <returns>POST HelloWorld Response</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost(""/api/HelloWorld"")]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(typeof(With201Response.Dtos.HelloWorldCreated), Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<With201Response.Dtos.HelloWorldCreated>> PostHelloWorld([Microsoft.AspNetCore.Mvc.FromBody]With201Response.Dtos.HelloWorldDto? body, System.Threading.CancellationToken cancellationToken)
        {
            var request = new With201Response.Requests.PostHelloWorldCommand(body);
            var response = await _mediator.Send(request, cancellationToken);
            return Created(""GetHelloWorld"", ""Api"", new {helloWorldId = response.Id}, response);
        }
    }
}";
}