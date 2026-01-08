using Microsoft.AspNetCore.Mvc;
using Termo.Api.UseCases;

namespace Termo.Api.Controllers;

[ApiController]
[Route("/games")]
public class GamesController(
    CreateGameUseCase createGameUseCase,
    GetGameByIdUseCase getGameByIdUseCase
) : ControllerBase
{
    [Route("{gameId:guid}")]
    public async Task<IActionResult> GetById(Guid gameId)
    {
        var game = await getGameByIdUseCase.ExecuteAsync(gameId);
        if (game is null)
            return NotFound(gameId);

        return Ok(game);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame()
    {
        var game = await createGameUseCase.ExecuteAsync();
        return Created($"/games/{game.Id}", game);
    }
}
