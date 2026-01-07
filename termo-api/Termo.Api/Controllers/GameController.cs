using Microsoft.AspNetCore.Mvc;
using Termo.Api.Repositories;

namespace Termo.Api.Controllers;

[ApiController]
[Route("/games")]
public class GamesController(IGameRepository gameRepository) : ControllerBase
{
    [Route("{gameId:guid}")]
    public async Task<IActionResult> GetById(Guid gameId)
    {
        var game = await gameRepository.GetByIdAsync(gameId);
        if (game is null)
            return NotFound(gameId);

        return Ok(game);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame()
    {
        var game = await gameRepository.CreateAsync();
        return Created($"/games/{game.Id}", game);
    }
}
