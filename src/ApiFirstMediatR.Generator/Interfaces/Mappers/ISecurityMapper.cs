namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface ISecurityMapper
{
    Security Map(IList<OpenApiSecurityRequirement>? security);
}
