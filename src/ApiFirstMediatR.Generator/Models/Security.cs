namespace ApiFirstMediatR.Generator.Models;

internal sealed class Security
{
    public Security()
    {
        Policies = new List<string>();
    }

    public IEnumerable<string>? Policies { get; set; }
}
