namespace ApiFirstMediatR.Generator.Models;

internal sealed class Parameter : Property
{
    public string? ParameterName { get; set; }
    public string? Attribute { get; set; }
    public override bool IsNullable => true;
}