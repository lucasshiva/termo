using System.IO.Abstractions.TestingHelpers;
using Shouldly;
using Termo.Api.Data;
using Termo.Api.Models;

namespace Termo.Api.Tests.Unit.WordTests;

public class WordLoaderTests
{
    private const string JsonFilePath = "Data/words.json";

    [Test]
    public void LoadWords_Throws_WhenJsonKeysAreInvalid()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(path: JsonFilePath, mockFile: new MockFileData(TestJson.InvalidKeys));
        var loader = new WordLoader(filePath: JsonFilePath, fileSystem: fileSystem);

        // Act / Assert
        Assert.Throws<InvalidDataException>(() => loader.LoadWords());
    }

    [Test]
    public void LoadWords_LoadsWords_WhenJsonIsValid()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(path: JsonFilePath, mockFile: new MockFileData(TestJson.Valid));
        var loader = new WordLoader(filePath: JsonFilePath, fileSystem: fileSystem);

        // Act
        var words = loader.LoadWords().ToList();
        words.Count.ShouldBe(2);
        Word firstWord = words[0];
        Word secondWord = words[1];

        // Assert
        firstWord.Value.ShouldBe("fogao"); // Always in lowercase
        firstWord.DisplayText.ShouldBe("FOGÃO"); // Converted to uppercase
        firstWord.Length.ShouldBe(5);

        secondWord.Value.ShouldBe("vineo"); // Always in lowercase
        secondWord.DisplayText.ShouldBe("VÍNEO"); // Converted to uppercase
        secondWord.Length.ShouldBe(5);
    }

    [Test]
    public void LoadWords_IgnoresWordsWithWrongLength()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(
            path: JsonFilePath,
            mockFile: new MockFileData(TestJson.WrongLengthWords)
        );
        var loader = new WordLoader(filePath: JsonFilePath, fileSystem: fileSystem);

        // Act
        var words = loader.LoadWords().ToList();

        // Assert
        words.Count.ShouldBe(1);
        Word firstWord = words.First();
        firstWord.Value.ShouldBe("casal");
        firstWord.DisplayText.ShouldBe("CASAL");
    }

    private static class TestJson
    {
        public const string Valid = """
            {
                "Words": ["fogão", "víneo"],
                "AccentMapping": { "fogao": "fogão", "vineo": "víneo" }
            }
            """;

        public const string InvalidKeys = """
            {
                "words": ["fogão"],
                "accent_mapping": { "fogao": "fogão" }
            }
            """;

        public const string WrongLengthWords = """
            {
                "Words": ["casal", "aves", "celular"],
                "AccentMapping": {}
            }
            """;
    }
}
