
namespace ApiFirstMediatR.Generator.Repositories;

internal sealed class OperationNamingRepository : IOperationNamingRepository
{
    private readonly IApiSpecRepository _apiSpecRepository;
    private readonly Lazy<OperationNamingDictionaries> _operationNamingDictionaries;

    public OperationNamingRepository(IApiSpecRepository apiSpecRepository)
    {
        _apiSpecRepository = apiSpecRepository;
        _operationNamingDictionaries = new Lazy<OperationNamingDictionaries>(ParseApiSpec);
    }

    public string? GetControllerNameByPath(string path)
    {
        _operationNamingDictionaries.Value.PathDictionary.TryGetValue(path, out var pathNaming);
        return pathNaming?.ControllerName;
    }

    public string? GetOperationNameByPathAndOperationType(string path, OperationType operationType)
    {
        _operationNamingDictionaries.Value.PathDictionary.TryGetValue(path, out var pathNaming);
        
        if (pathNaming is not null)
        {
            pathNaming.OperationDictionary.TryGetValue(operationType, out var operationNaming);
            return operationNaming?.OperationName;
        }

        return null;
    }

    public string? GetOperationNameByOperationId(string operationId)
    {
        _operationNamingDictionaries.Value.OperationIdDictionary.TryGetValue(operationId, out var pathNaming);
        return pathNaming?.OperationName;
    }

    public string? GetControllerNameByOperationId(string operationId)
    {
        _operationNamingDictionaries.Value.OperationIdDictionary.TryGetValue(operationId, out var pathNaming);
        return pathNaming?.ControllerName;
    }

    private OperationNamingDictionaries ParseApiSpec()
    {
        var apiSpecs = _apiSpecRepository.Get();
        var pathDictionary = new Dictionary<string, PathNaming>();
        var operationIdDictionary = new Dictionary<string, OperationNaming>();

        if (apiSpecs is null || apiSpecs.Length == 0)
        {
            return new OperationNamingDictionaries
            {
                OperationIdDictionary = new(),
                PathDictionary = new()
            };
        }

        foreach (var apiSpec in apiSpecs)
        {
            var controllerSpecs = apiSpec
                .Paths
            .GroupBy(p => GetControllerName(p.Key))
                .ToList();

            foreach (var controllerSpec in controllerSpecs)
            {
                var paths = controllerSpec.ToOpenApiPaths();

                foreach (var path in paths)
                {
                    var pathNaming = new PathNaming
                    {
                        ControllerName = controllerSpec.Key.ToPascalCase()
                    };

                    foreach (var operation in path.Value.Operations)
                    {
                        var endpointName = operation.Value.OperationId ?? $"{operation.Key.GetDisplayName()}{PathToEndpointName(path.Key)}";
                        var operationNaming = new OperationNaming
                        {
                            ControllerName = pathNaming.ControllerName,
                            OperationName = endpointName.ToCleanName().ToPascalCase()
                        };

                        pathNaming.OperationDictionary.Add(operation.Key, operationNaming);

                        if (operation.Value.OperationId is not null)
                        {
                            operationIdDictionary.Add(operation.Value.OperationId, operationNaming);
                        }
                    }

                    pathDictionary.Add(path.Key, pathNaming);
                }
            }
        }

        return new OperationNamingDictionaries
        {
            PathDictionary = pathDictionary,
            OperationIdDictionary = operationIdDictionary
        };
    }

    private static string GetControllerName(string path)
    {
        // TODO: Make this configurable
        return path
            .Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault() ?? "Default";
    }
    
    private static string PathToEndpointName(string path)
    {
        var pathParts  = path
            .Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(ps => ps.Replace("{", "").Replace("}", "").ToPascalCase());

        return string.Join("", pathParts);
    }
}

internal sealed class OperationNamingDictionaries
{
    public required Dictionary<string, PathNaming> PathDictionary { get; set; }
    public required Dictionary<string, OperationNaming> OperationIdDictionary { get; set; }
}

internal sealed class PathNaming
{
    public required string ControllerName { get; set; }
    public Dictionary<OperationType, OperationNaming> OperationDictionary { get; } = new();
}

internal sealed class OperationNaming
{
    public required string ControllerName { get; set; }
    public required string OperationName { get; set; }
}