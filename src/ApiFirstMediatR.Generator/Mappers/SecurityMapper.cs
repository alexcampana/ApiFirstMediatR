namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class SecurityMapper : ISecurityMapper
{
    public Security Map(IList<OpenApiSecurityRequirement>? security)
    {
        if (security is null || !security.Any())
            return new Security();

        var policies = new List<string>();
        foreach (var securityRequirement in security)
        {
            foreach (var kv in securityRequirement.Where(kv => kv.Key.Type == SecuritySchemeType.Http))
            {
                policies.AddRange(kv.Value);
            }
        }
        
        return new Security
        {
            Policies = policies
        };
    }
}