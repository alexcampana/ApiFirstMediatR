namespace ApiFirstMediatR.Generator.Models;

internal class Response
{
    public string? BodyType { get; set; }
    public HttpStatusCode? HttpStatusCode { get; set; }
    public IEnumerable<string>? Description { get; set; }
}