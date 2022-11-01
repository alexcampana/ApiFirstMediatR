namespace ApiFirstMediatR.Generator.Models;

internal sealed class Property
{
    public string? Name { get; set; }
    public bool IsNullable { get; set; } = true;
    public string? DataType { get; set; }
}