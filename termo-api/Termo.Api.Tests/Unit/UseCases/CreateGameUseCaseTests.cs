using NSubstitute;
using Shouldly;
using Termo.Api.Models;
using Termo.Api.Repositories;
using Termo.Api.UseCases;

namespace Termo.Api.Tests.Unit.UseCases;

public class CreateGameUseCaseTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
    private readonly IWordRepository _wordRepository = Substitute.For<IWordRepository>();

    [Test]
    public async Task CreateGameUseCase_ReturnsValidGame()
    {
        // Arrange
        var word = new Word("casal");
        _wordRepository.GetRandomWord().Returns(word);
        var useCase = new CreateGameUseCase(_gameRepository, _wordRepository);

        // Act
        var game = await useCase.ExecuteAsync();

        // Assert
        game.Word.ShouldBe(word);

        // Ensure the game is being saved.
        await _gameRepository.Received().AddAsync(game);
    }
}
