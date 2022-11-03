using Microsoft.CodeAnalysis.Text;

namespace ApiFirstMediatR.Generator.Extensions;

internal static class AdditionalTextExtensions
{
    // TODO: Implement this if it ever gets implemented: https://github.com/microsoft/OpenAPI.NET/issues/563
    public static Location GetLocation(this AdditionalText additionalText)
        => Location.Create(additionalText.Path, new TextSpan(), new LinePositionSpan());
}