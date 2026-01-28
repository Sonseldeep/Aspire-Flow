using GameStore.Api.Entities;

namespace GameStore.Api.DTOs;

public record GameCollectionDto
{
    public IEnumerable<GameDto> Items { get; init; }
}