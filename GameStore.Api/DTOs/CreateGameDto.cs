using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.DTOs;

public sealed record CreateGameDto(
    [Required,StringLength(50)] string Name,
    [Required,StringLength(50)] string Genre,
    [Range(1,1000)] decimal Price,
    DateOnly ReleaseDate,
    [Required,Url] string ImageUri
);
