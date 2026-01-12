using Shouldly;
using Termo.Api.Models;

namespace Termo.Api.Tests.Unit.WordTests;

public class WordTests
{
    [Test]
    public void Constructor_WithValidStrings_NormalizesValueAndDisplayText()
    {
        var word = new Word(value: "Fogao", displayText: "foGãO");
        word.Value.ShouldBe("fogao");
        word.DisplayText.ShouldBe("FOGÃO");
    }

    [Test]
    public void Constructor_WithNullDisplayText_UsesValueAsDisplay()
    {
        var word = new Word("casal");
        word.DisplayText.ShouldBe("CASAL");
    }

    [Test]
    public void Constructor_WithEmptyValue_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new Word("");
        });
    }

    [Test]
    [Arguments("cama")]
    [Arguments("frutas")]
    public void Constructor_WithWrongLengthValue_ThrowsException(string value)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new Word(value);
        });
    }
}
