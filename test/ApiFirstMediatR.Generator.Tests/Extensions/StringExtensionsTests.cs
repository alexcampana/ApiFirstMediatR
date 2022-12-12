using ApiFirstMediatR.Generator.Extensions;

namespace ApiFirstMediatR.Generator.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("TestName", "TestName")]
    [InlineData("Test2Name", "Test2Name")]
    [InlineData("2TestName", "two_TestName")]
    [InlineData("-TestName", "minus_TestName")]
    [InlineData("+TestName", "plus_TestName")]
    [InlineData("Test/Name", "Test_Name")]
    [InlineData(null, null)]
    public void NameCleaned(string? dirtyName, string? cleanName)
    {
        dirtyName.ToCleanName()
            .Should().Be(cleanName);
    }

    [Fact]
    public void EmptyName_ThrowsException()
    {
        "".Invoking(s => s.ToCleanName())
            .Should().Throw<NotSupportedException>();

    }

    [Theory]
    [InlineData("TestName", "TestName")]
    [InlineData("class", "@class")]
    [InlineData(null, null)]
    public void KeywordRemoved(string? dirtyName, string? cleanName)
    {
        dirtyName.ToKeywordSafeName()
            .Should().Be(cleanName);
    }
}