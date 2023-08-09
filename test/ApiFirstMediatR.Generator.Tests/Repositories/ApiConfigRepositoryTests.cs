using Microsoft.CodeAnalysis.Diagnostics;

namespace ApiFirstMediatR.Generator.Tests.Repositories;

public class ApiConfigRepositoryTests
{
    private readonly ApiConfigRepository _apiConfigRepository;
    private readonly AnalyzerConfigOptions _mockAnalyzerConfigOptions;
    private readonly ICompilation _mockICompilation;
    private readonly IDiagnosticReporter _mockDiagnosticReporter;
    
    public ApiConfigRepositoryTests()
    {
        _mockAnalyzerConfigOptions = Substitute.For<AnalyzerConfigOptions>();

        var analyzerConfigOptionsProvider = Substitute.For<AnalyzerConfigOptionsProvider>();
        analyzerConfigOptionsProvider
            .GlobalOptions
            .Returns(_mockAnalyzerConfigOptions);

        _mockICompilation = Substitute.For<ICompilation>();
        _mockICompilation
            .AnalyzerConfigOptions
            .Returns(analyzerConfigOptionsProvider);
        
        _mockDiagnosticReporter = Substitute.For<IDiagnosticReporter>();
        
        _apiConfigRepository = new ApiConfigRepository(_mockICompilation, _mockDiagnosticReporter);
    }
    
    [Fact]
    public void NoConfigValuesSet_HappyPath()
    {
        var compilation = CSharpCompilation.Create(null);
        _mockICompilation
            .Compilation
            .Returns(compilation);

        var apiConfig = _apiConfigRepository.Get();

        apiConfig.Namespace
            .Should().Be("ApiFirst");
        
        apiConfig.SerializationLibrary
            .Should().Be(SerializationLibrary.SystemTextJson);
    }
    
    [Fact]
    public void ConfigValuesSet_HappyPath()
    {
        var jsonLibrary = "Newtonsoft.Json";
        _mockAnalyzerConfigOptions
            .TryGetValue("build_property.ApiFirstMediatR_SerializationLibrary", out Arg.Any<string>()!)
            .Returns(x =>
            {
                x[1] = jsonLibrary; // This is the out value
                return true;
            });
        
        var compilation = CSharpCompilation.Create("ApiFirstChanged");
        _mockICompilation
            .Compilation
            .Returns(compilation);

        var apiConfig = _apiConfigRepository.Get();

        apiConfig.Namespace
            .Should().Be("ApiFirstChanged");
        
        apiConfig.SerializationLibrary
            .Should().Be(SerializationLibrary.NewtonsoftJson);
    }

    [Fact]
    public void InvalidSerializationLibrary_ThrowsDiagnostic()
    {
        var jsonLibrary = "BadJsonSerializer";
        _mockAnalyzerConfigOptions
            .TryGetValue("build_property.ApiFirstMediatR_SerializationLibrary", out Arg.Any<string>()!)
            .Returns(x =>
            {
                x[1] = jsonLibrary; // This is the out value
                return true;
            });
        
        var compilation = CSharpCompilation.Create(null);
        _mockICompilation
            .Compilation
            .Returns(compilation);

        var apiConfig = _apiConfigRepository.Get();

        apiConfig.Namespace
            .Should().Be("ApiFirst");
        
        apiConfig.SerializationLibrary
            .Should().Be(SerializationLibrary.SystemTextJson);

        _mockDiagnosticReporter.Received(1).ReportDiagnostic(Arg.Any<Diagnostic>());
    }
}