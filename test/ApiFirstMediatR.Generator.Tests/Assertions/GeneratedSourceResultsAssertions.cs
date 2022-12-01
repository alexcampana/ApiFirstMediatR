using FluentAssertions.Collections;
using FluentAssertions.Execution;

namespace ApiFirstMediatR.Generator.Tests.Assertions;

public class GeneratedSourceResultsAssertions : GenericCollectionAssertions<IEnumerable<GeneratedSourceResult>, GeneratedSourceResult, GeneratedSourceResultsAssertions>
{
    public GeneratedSourceResultsAssertions(IEnumerable<GeneratedSourceResult> instances) : base(instances)
    {
    }

    /// <summary>
    /// Asserts that a collection of generated source results contains only one source with the given hint name
    /// and that the source is equivalent to the provided expected source text.
    /// </summary>
    /// <param name="hintName">The hint name to match.</param>
    /// <param name="sourceText">The expected source text.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because" />.</param>
    public AndConstraint<GeneratedSourceResultsAssertions> ContainEquivalentSyntaxTree(
        string hintName,
        string sourceText,
        string because = "",
        params object[] becauseArgs)
    {
        var success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:collection} to contain a single item{reason}, but found <null>.");

        GeneratedSourceResult? match = null;

        if (success)
        {
            var results = Subject?
                .Where(h => h.HintName == hintName)
                .ToList();
            
            switch (results?.Count)
            {
                case 0: // Fail, Collection is empty
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} to contain a single source with given hintName {0}, but the collection is empty.", hintName);
                    break;
                case 1: // Success Condition
                    match = results.Single();
                    break;
                default: // Fail, Collection contains more than a single item
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} to contain a single source with given hintName {0}, but found {1}.", hintName, Subject);
                    break;
            }

            if (match is not null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(CSharpSyntaxTree.ParseText(sourceText).IsEquivalentTo(match.Value.SyntaxTree))
                    .FailWith($"Expected source with given hintName {0} to be equivalent to the provided source, but it was not.{Environment.NewLine}{Environment.NewLine}Provided Source:{Environment.NewLine}{{1}}{Environment.NewLine}{Environment.NewLine}Expected Source:{Environment.NewLine}{{2}}", hintName, match.Value.SourceText, sourceText);
            }
        }

        return new AndConstraint<GeneratedSourceResultsAssertions>(this);
    }

    protected override string Identifier => "generatedSourceResult";
}

public static class GeneratedSourceResultsExtensions
{
    public static GeneratedSourceResultsAssertions Should(this IEnumerable<GeneratedSourceResult> instances)
    {
        return new GeneratedSourceResultsAssertions(instances);
    }
}