namespace ApiFirstMediatR.Generator.Models;

internal sealed class Security
{
    public SecuritySchemeType Type { get; set; }
    public string? Schema { get; set; }
    public string? BearerFormat { get; set; }
    public IEnumerable<string>? Scopes { get; set; }
}
