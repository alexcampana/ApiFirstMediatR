namespace ApiFirstMediatR.Generator;

[Generator]
public sealed class SourceGenerator : ISourceGenerator
{
    private static readonly IMutableContainer GeneratorContainer = Container.Create().Using<Glue>();
    
    public void Execute(GeneratorExecutionContext context)
    {
        using var container = GeneratorContainer
            .Create()
            .Bind<GeneratorExecutionContext>().As(Lifetime.ScopeRoot).To(ctx => context)
            .Create();

        // Entrypoint
        container.Resolve<IApiGenerator>().Generate();
        container.Dispose();
    }
    
    public void Initialize(GeneratorInitializationContext context)
    {
    }
}