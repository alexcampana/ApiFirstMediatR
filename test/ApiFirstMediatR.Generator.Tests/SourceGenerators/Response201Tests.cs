namespace ApiFirstMediatR.Generator.Tests.SourceGenerators;

public class Response201Tests : TestBase
{
    [Fact]
    public void ValidAPISpec_With201Response_GeneratesValidCode()
    {
        var result = RunGenerators("With201Response", ApiSpec);
        
        result.Diagnostics.Should().BeEmpty();
        result.GeneratedSources.Should()
                .ContainEquivalentSyntaxTree("default/Controllers_ApiController.g.cs", ExpectedController);
    }

    [Fact]
    public void ValidAPISpec_WithUnsupportedLink_ThrowsDiagnostic()
    {
        var result = RunGenerators("With201Response", ApiSpecInvalidLink);
        
        result.Diagnostics.Should().ContainSingle()
            .Which.Id.Should().Be(DiagnosticIdentifiers.ApiSpecFeatureNotSupported);
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
            return CreatedAtAction(""GetHelloWorld"", ""Api"", new {helloWorldId = response.Id}, response);
        }
    }
}";
}