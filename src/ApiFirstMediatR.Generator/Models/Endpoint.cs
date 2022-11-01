namespace ApiFirstMediatR.Generator.Models;

internal sealed class Endpoint
{
    public Endpoint(string name, string path, OperationType operationType)
    {
        Name = name.ToPascalCase();
        Path = path;
        OperationType = operationType;
    }

    public string Name { get; }
    public string Path { get; }
    public OperationType OperationType { get; }

    public string OperationName => OperationType.GetDisplayName().ToPascalCase();
}