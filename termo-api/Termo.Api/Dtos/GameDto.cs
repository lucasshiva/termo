using Termo.Api.Models;

namespace Termo.Api.Dtos;

public record GameDto
{
    public required Guid Id { get; init; }
    public required Word Word { get; init; }
}
