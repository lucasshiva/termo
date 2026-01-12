using Termo.Api.Models;

namespace Termo.Api.Dtos;

public record GuessDto
{
    public required string Value { get; init; }
    public IReadOnlyList<LetterEvaluation> Evaluations { get; init; } = [];
}
