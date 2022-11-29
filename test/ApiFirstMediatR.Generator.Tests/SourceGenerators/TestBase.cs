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
}