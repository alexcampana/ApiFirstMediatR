using ApiFirstMediatR.Generator.Extensions;
using Microsoft.CodeAnalysis.Text;

namespace ApiFirstMediatR.Generator;

[Generator]
public sealed class ApiSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var specFiles = context
            .AdditionalFiles
            .Where(f => f.Path.EndsWith(".yaml", StringComparison.InvariantCultureIgnoreCase) ||
                        f.Path.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        if (specFiles.Count == 0)
        {
            context.ReportDiagnostic(DiagnosticCatalog.ApiSpecFileNotFound());
            return;
        }

        var projectConfig = new ScriptObject();
        projectConfig.Add("namespace", context.Compilation.AssemblyName);

        foreach (var specFile in specFiles)
        {
            var fileContent = specFile.GetText(context.CancellationToken);

            if (fileContent is null || fileContent.Length == 0)
            {
                var diagnostic = DiagnosticCatalog.ApiSpecFileEmpty(specFile.GetLocation());
                context.ReportDiagnostic(diagnostic);
                continue;
            }

            var apiSpec = new OpenApiStringReader().Read(fileContent.ToString(), out var apiDiagnostic);

            if (apiDiagnostic.Errors.Any())
            {
                var diagnostic = DiagnosticCatalog.ApiSpecFileParsingError(specFile.GetLocation(), apiDiagnostic.Errors.First().Message);
                context.ReportDiagnostic(diagnostic);
                continue;
            }

            var dtos = DataTransferObjectMapper.Map(apiSpec);

            foreach (var dto in dtos)
            {
                var dtoSourceText = ApiTemplate.DataTransferObject.Generate(dto, projectConfig);
                context.AddSource($"Dtos_{dto.Name}.g.cs", dtoSourceText);
            }

            var controllers = ControllerMapper.Map(apiSpec);

            foreach (var controller in controllers)
            {
                if (controller.Endpoints != null)
                {
                    foreach (var endpoint in controller.Endpoints)
                    {
                        var endpointSourceText = ApiTemplate.MediatorRequest.Generate(endpoint, projectConfig);
                        context.AddSource($"MediatorRequests_{endpoint.MediatorRequestName}.g.cs", endpointSourceText);
                    }
                }

                var controllerSourceText = ApiTemplate.Controller.Generate(controller, projectConfig);
                context.AddSource($"Controllers_{controller.Name}.g.cs", controllerSourceText);
            }
        }
    }
    
    public void Initialize(GeneratorInitializationContext context)
    {
    }
}