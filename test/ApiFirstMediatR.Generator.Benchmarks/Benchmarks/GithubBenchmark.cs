namespace ApiFirstMediatR.Generator.Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class GithubBenchmark : BaseBenchmark
{
    private GeneratorDriver? _generatorDriver;
    private Compilation? _compilation;

    [GlobalSetup(Target = nameof(GenerateGithub))]
    public void GithubSetup()
    {
        var apiSpec = EmbeddedResource.GetContent("Specs/github_api.yaml");
        var additionalText = new AdditionalTextYml("github_api.yaml", apiSpec) as AdditionalText;
        _compilation = CreateCompilation("Github", "");

        var generator = new SourceGenerator();
        _generatorDriver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalText));
    }

    [Benchmark]
    public GeneratorDriver GenerateGithub() => _generatorDriver!.RunGenerators(_compilation!);

    [GlobalSetup(Target = nameof(GenerateSendGrid))]
    public void SendGridSetup()
    {
        var apiSpec = EmbeddedResource.GetContent("Specs/sendgrid_api.yaml");
        var additionalText = new AdditionalTextYml("sendgrid_api.yaml", apiSpec) as AdditionalText;
        _compilation = CreateCompilation("SendGrid", "");

        var generator = new SourceGenerator();
        _generatorDriver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalText));
    }

    [Benchmark]
    public GeneratorDriver GenerateSendGrid() => _generatorDriver!.RunGenerators(_compilation!);

    [GlobalSetup(Target = nameof(GeneratePetStore))]
    public void PetStoreSetup()
    {
        var apiSpec = EmbeddedResource.GetContent("Specs/petstore_api.yaml");
        var additionalText = new AdditionalTextYml("petstore_api.yaml", apiSpec) as AdditionalText;
        _compilation = CreateCompilation("PetStore", "");

        var generator = new SourceGenerator();
        _generatorDriver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalText));
    }

    [Benchmark]
    public GeneratorDriver GeneratePetStore() => _generatorDriver!.RunGenerators(_compilation!);
}