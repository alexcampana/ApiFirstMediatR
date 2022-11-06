namespace ApiFirstMediatR.Generator.Extensions;

internal static class StringExtensions
{
    public static IEnumerable<string> SplitOnNewLine(this string input)
    {
        return input.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
    }
}