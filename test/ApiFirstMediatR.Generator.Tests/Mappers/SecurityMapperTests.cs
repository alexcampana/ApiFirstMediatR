namespace ApiFirstMediatR.Generator.Tests.Mappers;

public class SecurityMapperTests
{
    private readonly ISecurityMapper _securityMapper;
    private readonly IDiagnosticReporter _mockDiagnosticReporter;
    
    public SecurityMapperTests()
    {
        _mockDiagnosticReporter = Substitute.For<IDiagnosticReporter>();

        _securityMapper = new SecurityMapper(_mockDiagnosticReporter);
    }

    [Fact]
    public void ValidSecurityPath_HappyPath()
    {
        var securityRequirements = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    },
                    new List<string>
                    {
                        "GetResource"
                    }
                }
            }
        };

        var security = _securityMapper.Map(securityRequirements);

        security.Policies.Should().NotBeNull();
        security.Policies.Should().ContainSingle()
            .Which.Should().Be("GetResource");
    }
    
    [Fact]
    public void Unsupported_ThrowsDiagnostic()
    {
        var securityRequirements = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                Scopes = new Dictionary<string, string>
                                {
                                    { "get:test", "description" }
                                }
                            }
                        }
                    },
                    new List<string>
                    {
                        "get:test"
                    }
                }
            }
        };
        
        var security = _securityMapper.Map(securityRequirements);

        security.Policies.Should().NotBeNull();
        security.Policies.Should().BeEmpty();
        _mockDiagnosticReporter.Received(1).ReportDiagnostic(Arg.Any<Diagnostic>());
    }
}