namespace ApiFirstMediatR.Generator.Mappers;

internal sealed class SecurityMapper : ISecurityMapper
{
    private readonly IDiagnosticReporter _diagnosticReporter;

    public SecurityMapper(IDiagnosticReporter diagnosticReporter)
    {
        _diagnosticReporter = diagnosticReporter;
    }
    
    public Security Map(IList<OpenApiSecurityRequirement>? security)
    {
        if (security is null || !security.Any())
            return new Security();

        var policies = new List<string>();
        foreach (var securityRequirement in security)
        {
            foreach (var kv in securityRequirement)
            {
                if (kv.Key.Type == SecuritySchemeType.Http)
                {
                    policies.AddRange(kv.Value);
                }
                else
                {
                    // TODO: Implement location finder so we can point to the exact location in the api spec file
                    _diagnosticReporter.ReportDiagnostic(DiagnosticCatalog.ApiSpecFeatureNotSupported(Location.None, $"Security: {kv.Key.Name} {string.Join(", ", kv.Value)}"));
                }
            }
        }
        
        return new Security
        {
            Policies = policies
        };
    }
}