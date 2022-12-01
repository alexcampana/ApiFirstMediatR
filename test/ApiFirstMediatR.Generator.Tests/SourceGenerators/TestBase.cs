namespace ApiFirstMediatR.Generator.Tests.SourceGenerators;

public abstract class TestBase
{
    protected static Compilation CreateCompilation(string source)
        => CreateCompilation("compilation", source);
    
    protected static Compilation CreateCompilation(string assemblyName, string source)
        => CSharpCompilation.Create(assemblyName,
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    
    protected class AdditionalTextYml : AdditionalText
    {
        private readonly string _text;

        public AdditionalTextYml(string path, string text)
        {
            Path = path;
            _text = text;
        }

        public override string Path { get; }

        public override SourceText GetText(CancellationToken cancellationToken = default)
        {
            return SourceText.From(_text);
        }
    }
    
    protected GeneratorRunResult RunGenerators(string assemblyName, string source, string specFileName, string specFileContent)
    {
        var inputCompilation = CreateCompilation(assemblyName, source);
        var additionalText = new AdditionalTextYml(specFileName, specFileContent) as AdditionalText;
        var generator = new SourceGenerator();
        var driver = CSharpGeneratorDriver
            .Create(generator)
            .AddAdditionalTexts(ImmutableArray.Create(additionalText))
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out _);

        var results = driver.GetRunResult().Results;
        return results.Should().ContainSingle().Subject;
    }

    protected GeneratorRunResult RunGenerators(string assemblyName, string specFileContent) =>
        RunGenerators(assemblyName, "", "api_spec.yml", specFileContent);
}