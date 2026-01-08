using Termo.Api.Dtos;
using Termo.Api.Repositories;

namespace Termo.Api.UseCases;

public class CreateGameUseCase(IGameRepository gameRepository, IWordRepository wordRepository)
{
    public async Task<GameDto> ExecuteAsync()
    {
        var word = wordRepository.GetRandomWord();
        var game = new GameDto { Id = Guid.NewGuid(), Word = word };
        await gameRepository.AddAsync(game);
        return game;
    }
}
