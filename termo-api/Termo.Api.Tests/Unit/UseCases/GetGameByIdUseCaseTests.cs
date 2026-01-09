using NSubstitute;
using Shouldly;
using Termo.Api.Dtos;
using Termo.Api.Models;
using Termo.Api.Repositories;
using Termo.Api.UseCases;

namespace Termo.Api.Tests.Unit.UseCases;

public class GetGameByIdUseCaseTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();

    [Test]
    public async Task GetGameByIdUseCase_ReturnsExistingGame()
    {
        // Arrange
        var game = new GameDto { Id = Guid.NewGuid(), Word = new Word("casal") };
        _gameRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(game);
        var useCase = new GetGameByIdUseCase(_gameRepository);

        // Act
        var foundGame = await useCase.ExecuteAsync(game.Id);
        foundGame.ShouldNotBeNull();
        await _gameRepository.Received().GetByIdAsync(game.Id);
        foundGame.Id.ShouldBe(game.Id);
        foundGame.Word.ShouldBe(game.Word);
    }
}
