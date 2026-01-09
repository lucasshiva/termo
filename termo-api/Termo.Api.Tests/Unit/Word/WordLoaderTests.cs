using System.IO.Abstractions.TestingHelpers;
using Shouldly;
using Termo.Api.Data;

namespace Termo.Api.Tests.Unit;

public class WordLoaderTests
{
    private const string JsonFilePath = "Data/words.json";

    [Test]
    public void LoadWords_Throws_WhenJsonKeysAreInvalid()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(JsonFilePath, new MockFileData(TestJson.InvalidKeys));
        var loader = new WordLoader(JsonFilePath, fileSystem);

        // Act / Assert
        Assert.Throws<InvalidDataException>(() => loader.LoadWords());
    }

    [Test]
    public void LoadWords_LoadsWords_WhenJsonIsValid()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(JsonFilePath, new MockFileData(TestJson.Valid));
        var loader = new WordLoader(JsonFilePath, fileSystem);

        // Act
        var word = loader.LoadWords().Single();

        // Assert
        word.Value.ShouldBe("fogao"); // Always in lowercase
        word.DisplayText.ShouldBe("FOGÃO"); // Converted to uppercase
        word.Length.ShouldBe(5);
    }

    private static class TestJson
    {
        public const string Valid = """
            {
                "Words": ["fogao"],
                "AccentMapping": { "fogao": "fogão" }
            }
            """;

        public const string InvalidKeys = """
            {
                "words": ["fogao"],
                "accentMapping": { "fogao": "fogão" }
            }
            """;
    }
}
