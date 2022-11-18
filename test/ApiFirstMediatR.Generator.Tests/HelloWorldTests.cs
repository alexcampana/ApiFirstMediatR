namespace ApiFirstMediatR.Generator.Tests;

public class HelloWorldTests : TestBase
{
    [Theory]
    [InlineData("api_spec3.yml", Yaml3ApiSpec)]
    [InlineData("api_spec3.json", Json3ApiSpec)]
    [InlineData("api_spec2.yml", Yaml2ApiSpec)]
    [InlineData("api_spec3.json", Json2ApiSpec)]
    public void ValidateSpec(string fileName, string fileContents)
    {
        var code = "namespace HelloWorld;";
        var inputCompilation = CreateCompilation("HelloWorld", code);
        var additionalText = new AdditionalTextYml(fileName, fileContents) as AdditionalText;

        var generator = new SourceGenerator();
        var driver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalText))
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
                out var diagnostics);
        
        Assert.Empty(diagnostics);
        
        var runResult = driver.GetRunResult();
        Assert.Single(runResult.Results);
        
        var generatedSources = runResult.Results.First().GeneratedSources;
        Assert.Equal(3, generatedSources.Length);

        var dtos = generatedSources.Where(g => g.HintName == "Dtos_HelloWorldDto.g.cs").ToList();
        Assert.Single(dtos);

        var mediatrRequests = generatedSources.Where(g => g.HintName == "MediatorRequests_GetHelloWorldQuery.g.cs").ToList();
        Assert.Single(mediatrRequests);

        var controllers = generatedSources.Where(g => g.HintName == "Controllers_ApiController.g.cs").ToList();
        Assert.Single(controllers);

        var dtoExpectedResult = CSharpSyntaxTree.ParseText(ExpectedDto);
        Assert.True(dtoExpectedResult.IsEquivalentTo(dtos.First().SyntaxTree));

        var mediatrExpectedResult = CSharpSyntaxTree.ParseText(ExpectedMediatorRequest);
        Assert.True(mediatrExpectedResult.IsEquivalentTo(mediatrRequests.First().SyntaxTree));

        var controllerExpectedResult = CSharpSyntaxTree.ParseText(ExpectedController);
        Assert.True(controllerExpectedResult.IsEquivalentTo(controllers.First().SyntaxTree));
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