using Termo.Api.Dtos;
using Termo.Api.Repositories;

namespace Termo.Api.UseCases;

public class GetGameByIdUseCase(IGameRepository gameRepository)
{
    public Task<GameDto?> ExecuteAsync(Guid id)
    {
        return gameRepository.GetByIdAsync(id);
    }
}
