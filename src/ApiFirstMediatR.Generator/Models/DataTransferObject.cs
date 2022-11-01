namespace ApiFirstMediatR.Generator.Models;

internal sealed class DataTransferObject
{
    public string? Name { get; set; }
    public string? InheritedDto { get; set; }
    public IEnumerable<Property>? Properties { get; set; }
}