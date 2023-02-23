namespace ApiFirstMediatR.Generator.Interfaces.Mappers;

internal interface ISecurityMapper
{
    IEnumerable<Security> Map(IList<OpenApiSecurityRequirement>? security);
}
