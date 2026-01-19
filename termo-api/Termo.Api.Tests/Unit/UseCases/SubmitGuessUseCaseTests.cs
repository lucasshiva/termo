using Shouldly;
using Termo.Api.Dtos;
using Termo.Api.Guesses;
using Termo.Api.Models;
using Termo.Api.Repositories;
using Termo.Api.UseCases;

namespace Termo.Api.Tests.Unit.UseCases;

public class SubmitGuessUseCaseTests
{
    private readonly InMemoryGameRepository _gameRepository = new();
    private readonly IGuessEvaluator _guessEvaluator = new GuessEvaluator();

    [Test]
    public async Task SubmitGuessUseCase_WithCorrectWord_WinsGame()
    {
        // Arrange
        var targetWord = new Word("casal");
        var fakeGame = new GameDto { Id = Guid.NewGuid(), Word = targetWord };
        await _gameRepository.AddAsync(fakeGame);
        var useCase = new SubmitGuessUseCase(
            gameRepository: _gameRepository,
            guessEvaluator: _guessEvaluator
        );

        // Act
        GameDto gameDto = await useCase.ExecuteAsync(gameId: fakeGame.Id, guess: "casal");

        // Assert
        gameDto.State.ShouldBe(GameState.Won);
        gameDto.Guesses[0].Value.ShouldBe("casal");
        gameDto
            .Guesses[0]
            .Evaluations.Select(e => e.State)
            .ShouldAllBe(s => s == LetterState.Correct);
    }

    [Test]
    public async Task SubmitGuessUseCase_WithWrongWords_LosesGame()
    {
        // Arrange
        var targetWord = new Word("casal");
        var fakeGame = new GameDto { Id = Guid.NewGuid(), Word = targetWord };
        await _gameRepository.AddAsync(fakeGame);
        var useCase = new SubmitGuessUseCase(
            gameRepository: _gameRepository,
            guessEvaluator: _guessEvaluator
        );

        // Act
        GameDto? gameDto = null;
        // Guess wrong word on purpose
        for (var i = 0; i < fakeGame.MaxGuesses; i++)
            gameDto = await useCase.ExecuteAsync(gameId: fakeGame.Id, guess: "placa");

        // Assert
        gameDto.ShouldNotBeNull();
        gameDto.Guesses.Count.ShouldBe(gameDto.MaxGuesses);
        gameDto.State.ShouldBe(GameState.Lost);
    }

    [Test]
    public async Task SubmitGuessUseCase_WithAccentedWord_WinsGame()
    {
        // Arrange
        var targetWord = new Word("vineo", "VÍNEO");
        var fakeGame = new GameDto { Id = Guid.NewGuid(), Word = targetWord };
        await _gameRepository.AddAsync(fakeGame);
        var useCase = new SubmitGuessUseCase(
            gameRepository: _gameRepository,
            guessEvaluator: _guessEvaluator
        );

        // Act
        GameDto gameDto = await useCase.ExecuteAsync(gameId: fakeGame.Id, guess: "vineo");

        // Assert
        gameDto.Guesses[0].Value.ShouldBe("vineo");
        gameDto
            .Guesses[0]
            .Evaluations.Select(e => e.State)
            .ShouldAllBe(s => s == LetterState.Correct);
        gameDto.State.ShouldBe(GameState.Won);
    }
}
