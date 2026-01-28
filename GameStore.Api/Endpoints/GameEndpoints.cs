using GameStore.Api.DTOs;
using GameStore.Api.Mappings;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    private const string GetGameById = "GetGameById";
    public static void MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/games").WithTags("Games");

        group.MapGet("/", async (IGameRepository repo, CancellationToken ct) =>
        {
            var games = await repo.GetAllAsync(ct);

            var gamesCollectionDto = new GameCollectionDto
            {
                Items = games.Select(g => g.ToDto())
            };
            return Results.Ok(gamesCollectionDto);
        });

        group.MapGet("/{id:guid}", async (Guid id, IGameRepository repo, CancellationToken ct) =>
        {
            var game = await repo.GetByIdAsync(id, ct);
            return game is not null ? Results.Ok(game.ToDto()) : Results.NotFound();
        }).WithName(GetGameById);

        group.MapPost("/", async (CreateGameDto dto, IGameRepository repo, CancellationToken ct) =>
        {
            var game = dto.ToEntity();
            await repo.AddAsync(game, ct);
            
            return Results.CreatedAtRoute(GetGameById, new { id = game.Id }, game.ToDto());
        });

        group.MapPut("/{id:guid}",
            async (Guid id, UpdateGameDto dto, IGameRepository repo, CancellationToken ct) =>
            {
                var game = await repo.GetByIdAsync(id, ct);
                if (game is null)
                {
                    return Results.NotFound();
                }
                game.UpdateFromDto(dto);
                await repo.UpdateAsync(game, ct);
                
                return Results.NoContent();
            });

        group.MapDelete("/{id:guid}", async (Guid id, IGameRepository repo, CancellationToken ct) =>
        {
            var game = await repo.GetByIdAsync(id, ct);
            if (game is null)
            {
                return Results.NotFound();
            }
            await repo.DeleteAsync(game, ct);
            return Results.NoContent();
        });
    }
}