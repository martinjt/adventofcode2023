using System.Text.RegularExpressions;
using Xunit;

namespace day3;

public class TestClass
{

    [Fact]
    public void RegexTest()
    {
        var line = "123...456!";
        var numberRegex = new Regex(@"\d+");
        var symbolRegex = new Regex(@"[-!$%^&*()_+|~=`{}\[\]:"";'<>?,\/]");
        var matches = numberRegex.Matches(line);
        Assert.Equal(2, matches.Count);
        Assert.Equal("123", matches[0].Value);
        Assert.Equal("456", matches[1].Value);
        Assert.Equal(0, matches[0].Index);
        Assert.Equal(3, matches[1].Length);

        Assert.Matches(symbolRegex, line);

        var lineWithAsterix = "...*...";
        Assert.Matches(symbolRegex, lineWithAsterix);
        var lineWithoutSymbols = ".......";
        Assert.DoesNotMatch(symbolRegex, lineWithoutSymbols);

        var asterixRegex = new Regex(@"\*");
        Assert.Matches(asterixRegex, lineWithAsterix);
    }

    [Fact]
    public void NextCharacterTest_Basic()
    {
        var lines = new [] { "123!..." };
        var regex = new Regex(@"\d+");
        var matches = regex.Matches(lines[0]);
        Assert.True(matches[0].NextCharacterIsSymbol(lines[0]));
    }
    [Fact]
    public void NextCharacterTest_EndOfString()
    {
        var lines = new [] { "...123" };
        var regex = new Regex(@"\d+");
        var matches = regex.Matches(lines[0]);
        Assert.False(matches[0].NextCharacterIsSymbol(lines[0]));
    }

    [Fact]
    public void PreviousCharacterTest_Basic()
    {
        var lines = new [] { "..!123" };
        var regex = new Regex(@"\d+");
        var matches = regex.Matches(lines[0]);
        Assert.True(matches[0].PreviousCharacterIsSymbol(lines[0]));
    }
    [Fact]
    public void PreviousCharacterTest_StartOfString()
    {
        var lines = new [] { "123!..." };
        var regex = new Regex(@"\d+");
        var matches = regex.Matches(lines[0]);
        Assert.False(matches[0].PreviousCharacterIsSymbol(lines[0]));
    }

}
