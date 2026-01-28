namespace GameStore.Api.DTOs;

public sealed record GameDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate,
    string ImageUri
);