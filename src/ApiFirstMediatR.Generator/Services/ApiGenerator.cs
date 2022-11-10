namespace ApiFirstMediatR.Generator.Services;

internal sealed class ApiGenerator : IApiGenerator
{
    private readonly GeneratorExecutionContext _context;
    private readonly IRepository<DataTransferObject> _dtoRepository;
    private readonly IRepository<Controller> _controllerRepository;

    public ApiGenerator(GeneratorExecutionContext context, IRepository<DataTransferObject> dtoRepository, IRepository<Controller> controllerRepository)
    {
        _context = context;
        _dtoRepository = dtoRepository;
        _controllerRepository = controllerRepository;
    }

    public void Generate()
    {
        var projectConfig = new ScriptObject();
        projectConfig.Add("namespace", _context.Compilation.AssemblyName);
        
        var dtos = _dtoRepository.Get();

        foreach (var dto in dtos)
        {
            var dtoSourceText = ApiTemplate.DataTransferObject.Generate(dto, projectConfig);
            _context.AddSource($"Dtos_{dto.Name}.g.cs", dtoSourceText);
        }

        var controllers = _controllerRepository.Get();

        foreach (var controller in controllers)
        {
            if (controller.Endpoints != null)
            {
                foreach (var endpoint in controller.Endpoints)
                {
                    var endpointSourceText = ApiTemplate.MediatorRequest.Generate(endpoint, projectConfig);
                    _context.AddSource($"MediatorRequests_{endpoint.MediatorRequestName}.g.cs", endpointSourceText);
                }
            }

            var controllerSourceText = ApiTemplate.Controller.Generate(controller, projectConfig);
            _context.AddSource($"Controllers_{controller.Name}.g.cs", controllerSourceText);
        }
    }
}