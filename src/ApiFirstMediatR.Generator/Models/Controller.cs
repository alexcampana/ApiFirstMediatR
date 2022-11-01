namespace ApiFirstMediatR.Generator.Models;

internal sealed class Controller
{
    public string? Name { get; set; }
    public IEnumerable<Endpoint>? Endpoints { get; set; }
}