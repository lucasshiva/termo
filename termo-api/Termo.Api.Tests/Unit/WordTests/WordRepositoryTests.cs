using Shouldly;
using Termo.Api.Models;
using Termo.Api.Repositories;

namespace Termo.Api.Tests.Unit.WordTests;

public class WordRepositoryTests
{
    [Test]
    public void GetRandomWord_ReturnsAValidWord()
    {
        // Arrange
        List<Word> words = [new(value: "fogao", displayText: "fogão")];
        var repo = new WordRepository(words);

        // Act
        Word chosenWord = repo.GetRandomWord();

        // Assert
        chosenWord.ShouldBe(words.First());
    }

    [Test]
    public void FindWord_WithWrongInput_ReturnsNull()
    {
        // Arrange
        List<Word> words = [new(value: "fogao", displayText: "fogão")];
        const string input = "casal";
        var repo = new WordRepository(words);

        // Act
        Word? word = repo.FindWord(input);

        // Assert
        word.ShouldBeNull();
    }

    [Test]
    public void FindWord_WithCorrectInput_ReturnsTheCorrectWord()
    {
        // Arrange
        List<Word> words = [new(value: "fogao", displayText: "fogão")];
        const string input = "fogao";
        var repo = new WordRepository(words);

        // Act
        Word? word = repo.FindWord(input);

        // Assert
        word.ShouldNotBeNull();
        word.Value.ShouldBe(input);
        word.DisplayText.ShouldBe("FOGÃO");
    }
}
