namespace GameStore.Api.Entities;

public sealed class Game
{
    public Guid Id { get; set; }
    public required string  Name { get; set; } = string.Empty;
    public required string  Genre { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public required string ImageUri { get; set; } = string.Empty;
}