using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using SharpYaml.Serialization;

namespace ApiFirstMediatR.Generator.Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class ApiBenchmark : BaseBenchmark
{
    private GeneratorDriver? _generatorDriver;
    private Compilation? _compilation;
    private string? _apiSpecRaw;
    
    [Params("github_api", "sendgrid_api", "petstore_api")]
    public string? ApiSpecFile { get; set; }
    
    [Params("json", "yaml")]
    public string? ApiSpecFormat { get; set; }
    
    [GlobalSetup]
    public void BenchmarkSetup()
    {
        _apiSpecRaw = EmbeddedResource.GetContent($"Specs/{ApiSpecFile}.{ApiSpecFormat}");
        var additionalText = new AdditionalTextYml("github_api.yaml", _apiSpecRaw) as AdditionalText;
        _compilation = CreateCompilation("Github", "");

        var generator = new SourceGenerator();
        _generatorDriver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalText));
    }

    [Benchmark]
    public GeneratorDriver GenerateApi() => _generatorDriver!.RunGenerators(_compilation!);

    [Benchmark]
    public OpenApiDocument? ParseApiSpec() => new OpenApiStringReader().Read(_apiSpecRaw, out _);

    [Benchmark]
    public YamlDocument DeserializeApiSpec() => ParseYamlString(_apiSpecRaw!);

    private static YamlDocument ParseYamlString(string yamlString)
    {
        var reader = new StringReader(yamlString);
        var yamlStream = new YamlStream();
        yamlStream.Load(reader);

        var yamlDocument = yamlStream.Documents.First();
        return yamlDocument;
    }
}