using Termo.Api.Dtos;
using Termo.Api.Repositories;

namespace Termo.Api.UseCases;

public class CreateGameUseCase(IGameRepository gameRepository)
{
    public async Task<GameDto> ExecuteAsync()
    {
        var game = new GameDto { Id = Guid.NewGuid() };
        await gameRepository.AddAsync(game);
        return game;
    }
}
