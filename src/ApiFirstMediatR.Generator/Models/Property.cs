namespace ApiFirstMediatR.Generator.Models;

internal class Property
{
    public string? Name { get; set; }
    public string? JsonName { get; set; }
    public virtual bool IsNullable { get; set; } = true;
    public string? DataType { get; set; }
}