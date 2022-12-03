using ApiFirstMediatR.Generator.Services;

namespace ApiFirstMediatR.Generator;

[Generator]
public sealed class SourceGenerator : ISourceGenerator
{
    private static readonly IMutableContainer GeneratorContainer = Container.Create().Using<Glue>();
    
    public void Execute(GeneratorExecutionContext context)
    {
        using var container = GeneratorContainer
            .Create()
            .Bind<GeneratorExecutionContextWrapper, ISources, IDiagnosticReporter, ICompilation>().As(Lifetime.ScopeRoot).To(ctx => new GeneratorExecutionContextWrapper(context))
            .Create();

        // Entrypoint
        var generators = container.Resolve<IEnumerable<IApiGenerator>>();
        
        foreach (var generator in generators)
            generator.Generate();
    }
    
    public void Initialize(GeneratorInitializationContext context)
    {
    }
}