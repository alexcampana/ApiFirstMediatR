using ApiFirstMediatR.Generator.Services;
using Microsoft.OpenApi.Any;
using System.Collections.Concurrent;

namespace ApiFirstMediatR.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ApiAnalyzer : DiagnosticAnalyzer
{
    private static readonly IMutableContainer AnalyzerContainer = Container.Create().Using<Glue>();
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticCatalog.ApiMissingImplementationDescriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterCompilationStartAction(AnalyzeApiSpec);
    }

    private static void AnalyzeApiSpec(CompilationStartAnalysisContext context)
    {
        var handlerBag = new ConcurrentBag<string>();
        var endpointBag = new ConcurrentBag<RequestLocation>();

        context.RegisterAdditionalFileAction(fileContext =>
        {
            using var container = AnalyzerContainer
                .Create()
                .Bind<IDiagnosticReporter>().To<AnalyzerDiagnosticReporter>()
                .Bind<AdditionalFileAnalysisContextWrapper, ICompilation>().As(Lifetime.ScopeRoot).To(ctx => new AdditionalFileAnalysisContextWrapper(fileContext))
                .Create();

            var endpointMapper = container.Resolve<IEndpointMapper>();

            var fileContent = fileContext.AdditionalFile.GetText(fileContext.CancellationToken);

            if (fileContent is null || fileContent.Length == 0)
                return;

            var apiSpec = new OpenApiStringReader().Read(fileContent.ToString(), out var diagnostic);

            if (diagnostic.Errors.Any() || apiSpec is null)
                return;

            var ns = apiSpec.Extensions.TryGetValue("x-namespace", out var documentNamespace) ? ((OpenApiString)documentNamespace).Value : "default";

            var endpoints = endpointMapper.Map(apiSpec.Paths, ns);

            foreach (var endpoint in endpoints)
            {
                var implementationResponse = endpoint.Response?.BodyType is not null ? $", {endpoint.Response.BodyType}>" : ">";
                var implementation = $"{endpoint.MediatorRequestName}Handler : IRequestHandler<{endpoint.MediatorRequestName}{implementationResponse}";
                endpointBag.Add(new RequestLocation(endpoint.MediatorRequestName!, fileContext.AdditionalFile.GetLocation(), implementation));
            }
        });

        context.RegisterSymbolStartAction(analysisContext =>
        {
            var type = (INamedTypeSymbol)analysisContext.Symbol;

            if (type.TypeKind != TypeKind.Class || type.IsAbstract || type.IsStatic)
                return;

            if (type.Interfaces.Any(x => x.Name == "IRequestHandler" && x.TypeArguments.Length >= 1))
            {
                var typeInterface = type.Interfaces.First(x => x.Name == "IRequestHandler");
                var requestType = typeInterface.TypeArguments.First().Name;
                handlerBag.Add(requestType);
            }
        }, SymbolKind.NamedType);

        context.RegisterCompilationEndAction(endContext =>
        {
            foreach (var endpoint in endpointBag)
            {
                if (!handlerBag.Contains(endpoint.Name))
                    endContext.ReportDiagnostic(DiagnosticCatalog.ApiMissingImplementation(endpoint.Location, endpoint.Implementation));
            }
        });
    }

    private sealed class RequestLocation
    {
        public RequestLocation(string name, Location location, string implementation)
        {
            Name = name;
            Location = location;
            Implementation = implementation;
        }

        public string Name { get; }
        public Location Location { get; }
        public string Implementation { get; }
    }
}