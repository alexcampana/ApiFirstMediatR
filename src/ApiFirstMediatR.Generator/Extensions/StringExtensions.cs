using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp;

namespace ApiFirstMediatR.Generator.Extensions;

internal static class StringExtensions
{
    private static readonly ImmutableHashSet<string> Keywords = 
        SyntaxFacts.GetKeywordKinds().Select(SyntaxFacts.GetText).ToImmutableHashSet();

    private static readonly ImmutableDictionary<char, string> NumberConversions = new Dictionary<char, string>()
    {
        { '0', "zero_" },
        { '1', "one_" },
        { '2', "two_"},
        { '3', "three_" },
        { '4', "four_" },
        { '5', "five_" },
        { '6', "three_" },
        { '7', "three_" },
        { '8', "three_" },
        { '9', "three_" },
        { '-', "minus_" },
        { '+', "plus_" }
    }.ToImmutableDictionary();
    
    public static IEnumerable<string> SplitOnNewLine(this string? input)
    {
        return input?.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>();
    }

    public static string? ToCleanName(this string? name)
    {
        if (name is null)
            return name;

        if (name.Length == 0) // TODO: Throw diagnostic instead of exception
            throw new NotSupportedException("Name must have at least one character.");

        if (NumberConversions.TryGetValue(name[0], out var replacement))
        {
            name = $"{replacement}{name.Substring(1)}";
        }
        
        return Regex
            .Replace(name, @"(\s+|\+|&|'|\(|\)|<|>|#|\\|/)", "_");
    }

    public static string? ToKeywordSafeName(this string? name)
    {
        if (name is null)
            return name;

        return Keywords.Contains(name) ? $"@{name}" : name;
    }
}