namespace ApiFirstMediatR.Generator;

[Generator]
public class ApiSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var specFiles = context
            .AdditionalFiles
            .Where(f => f.Path.EndsWith(".yaml", StringComparison.InvariantCultureIgnoreCase) ||
                        f.Path.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        if (specFiles.Count == 0)
            return; // TODO: Throw a diagnostic here

        foreach (var specFile in specFiles)
        {
            var fileContent = specFile.GetText(context.CancellationToken);

            if (fileContent is null)
                continue; // TODO: Throw a diagnostic here

            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                fileContent.Write(writer);
            }

            stream.Position = 0;

            var apiSpec = new OpenApiStreamReader().Read(stream, out var diagnostic);

            if (diagnostic.Errors.Any())
                continue; // TODO: Throw a diagnostic here

            var controllers = ControllerMapper.Map(apiSpec);

            foreach (var controller in controllers)
            {
                var sourceText = ApiTemplate.Controller.Generate(controller);
                context.AddSource($"Controllers_{controller.Name}.g.cs", sourceText);
            }

            var dtos = DataTransferObjectMapper.Map(apiSpec);

            foreach (var dto in dtos)
            {
                var sourceText = ApiTemplate.DataTransferObject.Generate(dto);
                context.AddSource($"Dtos_{dto.Name}.g.cs", sourceText);
            }
        }
    }
    
    public void Initialize(GeneratorInitializationContext context)
    {
    }
}