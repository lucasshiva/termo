using Microsoft.AspNetCore.Mvc;
using Termo.Api.Dtos;
using Termo.Api.Requests;
using Termo.Api.UseCases;

namespace Termo.Api.Controllers;

[ApiController]
[Route("/games")]
public class GamesController(
    CreateGameUseCase createGameUseCase,
    GetGameByIdUseCase getGameByIdUseCase,
    SubmitGuessUseCase submitGuessUseCase
) : ControllerBase
{
    [Route("{gameId:guid}")]
    public async Task<IActionResult> GetById(Guid gameId)
    {
        GameDto? game = await getGameByIdUseCase.ExecuteAsync(gameId);
        if (game is null)
            return NotFound("Game not found");

        return Ok(game);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame()
    {
        GameDto game = await createGameUseCase.ExecuteAsync();
        return Created($"/games/{game.Id}", game);
    }

    [HttpPost("{gameId:guid}/guess")]
    public async Task<IActionResult> SubmitGuess(Guid gameId, [FromBody] SubmitGuessRequest request)
    {
        GameDto game = await submitGuessUseCase.ExecuteAsync(gameId, request.Guess);
        return Ok(game);
    }
}
