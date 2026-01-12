using System.Net;
using System.Net.Http.Json;
using Shouldly;
using Termo.Api.Dtos;
using Termo.Api.Models;
using Termo.Api.Requests;

namespace Termo.Api.Tests.Integration;

public class GamesControllerTests : ControllerTestsBase
{
    [Test]
    public async Task CreateGame_ReturnsGameDto()
    {
        // Arrange
        HttpClient client = Factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.PostAsync(requestUri: "/games", content: null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        GameDto gameDto = (
            await response.Content.ReadFromJsonAsync<GameDto>()
        ).ShouldBeOfType<GameDto>();
        Uri location = response.Headers.Location.ShouldNotBeNull();
        location.OriginalString.ShouldBe($"/games/{gameDto.Id}");
        gameDto.Word.ShouldNotBeNull();
        gameDto.Word.Value.ShouldBeOfType<string>();
        gameDto.Word.Value.All(char.IsLower).ShouldBeTrue();
        gameDto.Word.DisplayText.All(char.IsUpper).ShouldBeTrue();
        gameDto.Word.Length.ShouldBe(gameDto.Word.Value.Length);
    }

    [Test]
    public async Task GetById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        HttpClient client = Factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync($"/games/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetById_WithValidId_ReturnsGameDto()
    {
        // Arrange
        HttpClient client = Factory.CreateClient();
        GameDto game = await CreateGame(client);

        // Act
        HttpResponseMessage response = await client.GetAsync($"/games/{game.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        GameDto gameDto = (
            await response.Content.ReadFromJsonAsync<GameDto>()
        ).ShouldBeOfType<GameDto>();
        gameDto.Id.ShouldBe(game.Id);
        gameDto.Word.ShouldNotBeNull();
        gameDto.Word.Value.ShouldBeOfType<string>();
        gameDto.Word.Value.All(char.IsLower).ShouldBeTrue();
        gameDto.Word.DisplayText.All(char.IsUpper).ShouldBeTrue();
        gameDto.Word.Length.ShouldBe(gameDto.Word.Value.Length);
    }

    [Test]
    public async Task SubmitGuess_WithCorrectGuess_WinsGame()
    {
        // Arrange
        HttpClient client = Factory.CreateClient();
        GameDto game = await CreateGame(client);
        var request = new SubmitGuessRequest { Guess = FakeWordRepository.TargetWord.Value };

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync(
            requestUri: $"/games/{game.Id}/guess",
            value: request
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        GameDto updatedGame = (
            await response.Content.ReadFromJsonAsync<GameDto>()
        ).ShouldBeOfType<GameDto>();
        updatedGame.State.ShouldBe(GameState.Won);
    }

    [Test]
    public async Task SubmitGuess_WithWrongGuesses_LosesGame()
    {
        // Arrange
        HttpClient client = Factory.CreateClient();
        GameDto game = await CreateGame(client);

        // Act
        HttpResponseMessage? lastResponse = null;
        for (var i = 0; i < game.MaxGuesses; i++)
        {
            var request = new SubmitGuessRequest { Guess = FakeWordRepository.WrongWord.Value };
            lastResponse = await client.PostAsJsonAsync(
                requestUri: $"/games/{game.Id}/guess",
                value: request
            );
        }

        // Assert
        lastResponse.ShouldNotBeNull();
        lastResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        GameDto updatedGame = (
            await lastResponse.Content.ReadFromJsonAsync<GameDto>()
        ).ShouldBeOfType<GameDto>();
        updatedGame.State.ShouldBe(GameState.Lost);
        updatedGame.Guesses.Count.ShouldBe(game.MaxGuesses);
    }

    private static async Task<GameDto> CreateGame(HttpClient client)
    {
        HttpResponseMessage createResponse = await client.PostAsync(
            requestUri: "/games",
            content: null
        );
        return (await createResponse.Content.ReadFromJsonAsync<GameDto>())!;
    }
}
