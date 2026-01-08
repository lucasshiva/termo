using System.Net;
using System.Net.Http.Json;
using Shouldly;
using Termo.Api.Dtos;

namespace Termo.Api.Tests.Integration;

public class GamesControllerTests : ControllerTestsBase
{
    [Test]
    public async Task CreateGame_ReturnsGameDto()
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.PostAsync("/games", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var gameDto = (
            await response.Content.ReadFromJsonAsync<GameDto>()
        ).ShouldBeOfType<GameDto>();
        var location = response.Headers.Location.ShouldNotBeNull();
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
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/games/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetById_WithValidId_ReturnsGameDto()
    {
        // Arrange
        var client = Factory.CreateClient();
        var createResponse = await client.PostAsync("/games", null);
        var createdGame = await createResponse.Content.ReadFromJsonAsync<GameDto>();
        var gameId = createdGame!.Id;

        // Act
        var response = await client.GetAsync($"/games/{gameId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var gameDto = (
            await response.Content.ReadFromJsonAsync<GameDto>()
        ).ShouldBeOfType<GameDto>();
        gameDto.Id.ShouldBe(gameId);
        gameDto.Word.ShouldNotBeNull();
        gameDto.Word.Value.ShouldBeOfType<string>();
        gameDto.Word.Value.All(char.IsLower).ShouldBeTrue();
        gameDto.Word.DisplayText.All(char.IsUpper).ShouldBeTrue();
        gameDto.Word.Length.ShouldBe(gameDto.Word.Value.Length);
    }
}
