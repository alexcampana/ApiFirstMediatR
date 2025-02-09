namespace ApiFirstMediatR.Generator.Tests.SourceGenerators;

public class HelloWorldTests : TestBase
{
    [Theory]
    [InlineData("api_spec3.yml", Yaml3ApiSpec)]
    [InlineData("api_spec3.json", Json3ApiSpec)]
    [InlineData("api_spec2.yml", Yaml2ApiSpec)]
    [InlineData("api_spec3.json", Json2ApiSpec)]
    public void ValidateSpec(string fileName, string fileContents)
    {
        var result = RunGenerators(
            "HelloWorld",
            "namespace HelloWorld;",
            fileName, 
            fileContents);

        result.Diagnostics.Should().BeEmpty();
        result.GeneratedSources.Should().HaveCount(3)
            .And.ContainEquivalentSyntaxTree("default/Dtos_HelloWorldDto.g.cs", ExpectedDto)
            .And.ContainEquivalentSyntaxTree("default/MediatorRequests_GetHelloWorldQuery.g.cs", ExpectedMediatorRequest)
            .And.ContainEquivalentSyntaxTree("default/Controllers_ApiController.g.cs", ExpectedController);
    }
    
    private const string Yaml3ApiSpec = @"openapi: 3.0.1
info:
  title: HelloWorld API
  version: v1
paths:
  /api/HelloWorld:
    get:
      tags:
        - HelloWorld
      operationId: GetHelloWorld
      description: Gets a HelloWorld Message
      parameters: []
      responses:
        200:
          description: Hello world!
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/HelloWorldDto'
components:
  schemas:
    HelloWorldDto:
      type: object
      properties:
        message:
          type: string
          nullable: true
security: []";

    private const string Json3ApiSpec = @"{
  ""openapi"": ""3.0.1"",
    ""info"": {
        ""title"": ""HelloWorld API"",
        ""version"": ""v1""
    },
""paths"": {
    ""/api/HelloWorld"": {
        ""get"": {
            ""tags"": [
            ""HelloWorld""
                ],
            ""operationId"": ""GetHelloWorld"",
            ""description"": ""Gets a HelloWorld Message"",
            ""parameters"": [],
            ""responses"": {
                ""200"": {
                    ""description"": ""Hello world!"",
                    ""content"": {
                        ""application/json"": {
                            ""schema"": {
                                ""$ref"": ""#/components/schemas/HelloWorldDto""
                            }
                        }
                    }
                }
            }
        }
    }
},
""components"": {
    ""schemas"": {
        ""HelloWorldDto"": {
            ""type"": ""object"",
            ""properties"": {
                ""message"": {
                    ""type"": ""string"",
                    ""nullable"": true
                }
            }
        }
    }
},
""security"": []
}";

    private const string Yaml2ApiSpec = @"swagger: 2.0
info:
  title: HelloWorld API
  version: v1
basePath: /
produces:
  - application/json
paths:
  /api/HelloWorld:
    get:
      description: Gets a HelloWorld Message
      operationId: GetHelloWorld
      parameters: []
      tags:
        - HelloWorld
      responses:
        200:
          description: Hello world!
          schema:
            $ref: '#/definitions/HelloWorldDto'
definitions:
  HelloWorldDto:
    required: []
    properties:
      message:
        type: string
";

    private const string Json2ApiSpec = @"{
  ""swagger"": ""2.0"",
  ""info"": {
    ""title"": ""HelloWorld API"",
    ""version"": ""v1""
  },
  ""basePath"": ""/"",
  ""produces"": [
    ""application/json""
  ],
  ""paths"": {
    ""/api/HelloWorld"": {
      ""get"": {
        ""description"": ""Gets a HelloWorld Message"",
        ""operationId"": ""GetHelloWorld"",
        ""parameters"": [],
        ""tags"": [
          ""HelloWorld""
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""Hello world!"",
            ""schema"": {
              ""$ref"": ""#/definitions/HelloWorldDto""
            }
          }
        }
      }
    }
  },
  ""definitions"": {
    ""HelloWorldDto"": {
      ""required"": [],
      ""properties"": {
        ""message"": {
          ""type"": ""string""
        }
      }
    }
  }
}";

    private const string ExpectedDto = @"// <auto-generated/>
#nullable enable
#pragma warning disable CS8618

namespace HelloWorld.Dtos
{
    public class HelloWorldDto 
    {
        [System.Text.Json.Serialization.JsonPropertyName(""message"")]
        public string? Message { get; set; }
    }
}

#pragma warning restore CS8618";

    private const string ExpectedMediatorRequest = @"// <auto-generated/>
#nullable enable

namespace HelloWorld.Requests
{
    /// <summary>
    /// Gets a HelloWorld Message
    /// </summary>
    /// <returns>Hello world!</returns>
    public sealed class GetHelloWorldQuery : MediatR.IRequest<HelloWorld.Dtos.HelloWorldDto>
    {
        public GetHelloWorldQuery()
        {
        }
    }
}";

    private const string ExpectedController = @"// <auto-generated/>
#nullable enable

namespace HelloWorld.Controllers
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
        /// <returns>Hello world!</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet(""/api/HelloWorld"")]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(typeof(HelloWorld.Dtos.HelloWorldDto), Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<HelloWorld.Dtos.HelloWorldDto>> GetHelloWorld(System.Threading.CancellationToken cancellationToken)
        {
            var request = new HelloWorld.Requests.GetHelloWorldQuery();
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}";
}