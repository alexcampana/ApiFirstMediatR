namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class SecurityMapper : ISecurityMapper
{
    public IEnumerable<Security> Map(IList<OpenApiSecurityRequirement>? security)
    {
        if (security is null || !security.Any())
            return Array.Empty<Security>();
        
        var securities = new List<Security>();
        foreach(var securityRequirement in security)
        {
            foreach(var kv in securityRequirement)
            {
                securities.Add(new Security
                {
                    Type = kv.Key.Type,
                    Schema = kv.Key.Scheme,
                    BearerFormat= kv.Key.BearerFormat,
                    Scopes = kv.Value
                });
            }
        }
        return securities;
    }
}