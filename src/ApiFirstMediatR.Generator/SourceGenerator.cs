﻿namespace ApiFirstMediatR.Generator;

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
        var generators = container.Resolve<IEnumerable<IApiGenerator>>();
        
        foreach (var generator in generators)
            generator.Generate();
        
        container.Dispose();
    }
    
    public void Initialize(GeneratorInitializationContext context)
    {
    }
}