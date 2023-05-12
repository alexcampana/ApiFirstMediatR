using Microsoft.CodeAnalysis.Diagnostics;

namespace ApiFirstMediatR.Generator.Tests.Repositories;

public class ApiConfigRepositoryTests
{
    private readonly ApiConfigRepository _apiConfigRepository;
    private readonly Mock<AnalyzerConfigOptions> _mockAnalyzerConfigOptions;
    private readonly Mock<ICompilation> _mockICompilation;
    private readonly Mock<IDiagnosticReporter> _mockDiagnosticReporter;
    
    public ApiConfigRepositoryTests()
    {
        _mockAnalyzerConfigOptions = new Mock<AnalyzerConfigOptions>();

        var mockAnalyzerConfigOptionsProvider = new Mock<AnalyzerConfigOptionsProvider>();
        mockAnalyzerConfigOptionsProvider
            .Setup(m => m.GlobalOptions)
            .Returns(_mockAnalyzerConfigOptions.Object);

        _mockICompilation = new Mock<ICompilation>();
        _mockICompilation
            .Setup(m => m.AnalyzerConfigOptions)
            .Returns(mockAnalyzerConfigOptionsProvider.Object);
        
        _mockDiagnosticReporter = new Mock<IDiagnosticReporter>();
        
        _apiConfigRepository = new ApiConfigRepository(_mockICompilation.Object, _mockDiagnosticReporter.Object);
    }
    
    [Fact]
    public void NoConfigValuesSet_HappyPath()
    {
        var compilation = CSharpCompilation.Create(null);
        _mockICompilation
            .Setup(m => m.Compilation)
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
            .Setup(m => m.TryGetValue("build_property.ApiFirstMediatR_SerializationLibrary", out jsonLibrary))
            .Returns(true);
        
        var compilation = CSharpCompilation.Create("ApiFirstChanged");
        _mockICompilation
            .Setup(m => m.Compilation)
            .Returns(compilation);
        
        var operationGenerationMode = "MultipleClientsFromPathSegmentAndOperationId";
        _mockAnalyzerConfigOptions
            .Setup(m => m.TryGetValue("build_property.ApiFirstMediatR_OperationGenerationMode", out operationGenerationMode))
            .Returns(true);

        var apiConfig = _apiConfigRepository.Get();

        apiConfig.Namespace
            .Should().Be("ApiFirstChanged");
        
        apiConfig.SerializationLibrary
            .Should().Be(SerializationLibrary.NewtonsoftJson);

        apiConfig.OperationGenerationMode
            .Should().Be(OperationGenerationMode.MultipleClientsFromPathSegmentAndOperationId);
    }

    [Fact]
    public void InvalidSerializationLibrary_ThrowsDiagnostic()
    {
        var jsonLibrary = "BadJsonSerializer";
        _mockAnalyzerConfigOptions
            .Setup(m => m.TryGetValue("build_property.ApiFirstMediatR_SerializationLibrary", out jsonLibrary))
            .Returns(true);
        
        var compilation = CSharpCompilation.Create(null);
        _mockICompilation
            .Setup(m => m.Compilation)
            .Returns(compilation);

        var apiConfig = _apiConfigRepository.Get();

        apiConfig.Namespace
            .Should().Be("ApiFirst");
        
        apiConfig.SerializationLibrary
            .Should().Be(SerializationLibrary.SystemTextJson);
        
        _mockDiagnosticReporter.Verify(m => m.ReportDiagnostic(It.IsAny<Diagnostic>()));
        _mockDiagnosticReporter.VerifyNoOtherCalls();
    }
    
    [Fact]
    public void InvalidOperationGenerationMode_ThrowsDiagnostic()
    {
        var jsonLibrary = "Newtonsoft.Json";
        _mockAnalyzerConfigOptions
            .Setup(m => m.TryGetValue("build_property.ApiFirstMediatR_SerializationLibrary", out jsonLibrary))
            .Returns(true);
        
        var compilation = CSharpCompilation.Create(null);
        _mockICompilation
            .Setup(m => m.Compilation)
            .Returns(compilation);

        var operationGenerationMode = "BadOperationGenerationMode";
        _mockAnalyzerConfigOptions
            .Setup(m => m.TryGetValue("build_property.ApiFirstMediatR_OperationGenerationMode", out operationGenerationMode))
            .Returns(true);

        var apiConfig = _apiConfigRepository.Get();

        apiConfig.Namespace
            .Should().Be("ApiFirst");
        
        apiConfig.SerializationLibrary
            .Should().Be(SerializationLibrary.NewtonsoftJson);
        
        apiConfig.OperationGenerationMode
            .Should().Be(OperationGenerationMode.MultipleClientsFromPathSegmentAndOperationId);
        
        _mockDiagnosticReporter.Verify(m => m.ReportDiagnostic(It.IsAny<Diagnostic>()));
        _mockDiagnosticReporter.VerifyNoOtherCalls();
    }
}