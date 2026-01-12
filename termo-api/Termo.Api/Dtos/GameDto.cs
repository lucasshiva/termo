using Termo.Api.Models;

namespace Termo.Api.Dtos;

public record GameDto
{
    public required Guid Id { get; init; }
    public required Word Word { get; init; }
    public int MaxGuesses { get; init; } = 6;
    public IReadOnlyList<GuessDto> Guesses { get; init; } = [];
    public GameState State { get; set; } = GameState.InProgress;
}
