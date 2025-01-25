namespace ApiFirstMediatR.Generator.Models;

internal sealed class DataTransferObjectEnum
{
    public required string Name { get; set; }
    public required IEnumerable<DataTransferObjectEnumValue> EnumValues { get; set; }
    public string? Namespace { get; set; }
}