namespace ApiFirstMediatR.Generator.Models;

internal sealed class CreatedResponse : Response
{
    public required string EndpointName { get; set; }
    public required string ControllerName { get; set; }
    public required Dictionary<string, string> RouteMaps { get; set; }
}