using Microsoft.CodeAnalysis.Text;

namespace ApiFirstMediatR.Generator.Extensions;

internal static class AdditionalTextExtensions
{
    public static Location GetLocation(this AdditionalText additionalText)
        => Location.Create(additionalText.Path, new TextSpan(), new LinePositionSpan());
}