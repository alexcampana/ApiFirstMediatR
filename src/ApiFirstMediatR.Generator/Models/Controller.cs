namespace ApiFirstMediatR.Generator.Models;

internal sealed class Controller
{
    public Controller(string name, IEnumerable<Endpoint> endpoints)
    {
        Name = $"{name}Controller".ToPascalCase();
        Endpoints = endpoints;
    }

    public string Name { get; }
    public IEnumerable<Endpoint> Endpoints { get; }
}