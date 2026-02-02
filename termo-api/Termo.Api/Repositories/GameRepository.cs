using System.Collections.Concurrent;
using Termo.Api.Dtos;

namespace Termo.Api.Repositories;

public interface IGameRepository
{
    public Task<GameDto?> GetByIdAsync(Guid id);
    public Task AddAsync(GameDto game);
}

public class InMemoryGameRepository : IGameRepository
{
    private readonly ConcurrentDictionary<Guid, GameDto> _games = [];

    public Task<GameDto?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_games.GetValueOrDefault(id));
    }

    public Task AddAsync(GameDto game)
    {
        _games.AddOrUpdate(game.Id, game, (_, _) => game);
        return Task.FromResult(game);
    }
}
