using System.Text.Json;
using GameStore.Api.DTOs;
using GameStore.Api.Mappings;
using GameStore.Api.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    private const string GetGameById = "GetGameById";
    public static void MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/games").WithTags("Games");

        group.MapGet("/",
            async (IGameRepository repo,
            CancellationToken ct) =>
        {
            var games = await repo.GetAllAsync(ct);

            var gamesCollectionDto = new GameCollectionDto
            {
                Items = games.Select(g => g.ToDto())
            };
            return Results.Ok(gamesCollectionDto);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IGameRepository repo,
            IDistributedCache cache,
            ILogger<Program> logger,
            CancellationToken ct) =>
        {
            var cacheKey = $"game:{id}";

            var cachedJson = await cache.GetStringAsync(cacheKey, ct);
            if (cachedJson is not null)
            {
                logger.LogInformation("Cache HIT for game {GameId}", id);

                return Results.Ok(
                    JsonSerializer.Deserialize<GameDto>(cachedJson)
                );
            }

            logger.LogInformation("Cache MISS for game {GameId}", id);

            var game = await repo.GetByIdAsync(id, ct);
            if (game is null)
                return Results.NotFound();

            var gameDto = game.ToDto();

            await cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(gameDto),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                },
                ct);

            return Results.Ok(gameDto);

        }).WithName(GetGameById);


        group.MapPost("/", 
            async (CreateGameDto dto,
            IGameRepository repo, 
            CancellationToken ct) =>
        {
            var game = dto.ToEntity();
            await repo.AddAsync(game, ct);

            return Results.CreatedAtRoute(GetGameById, new { id = game.Id }, game.ToDto());
        }).RequireAuthorization();


        group.MapPut("/{id:guid}",
                async (Guid id,
                UpdateGameDto dto,
                IGameRepository repo,
                CancellationToken ct) =>
            {
                var game = await repo.GetByIdAsync(id, ct);
                if (game is null)
                {
                    return Results.NotFound();
                }

                game.UpdateFromDto(dto);
                await repo.UpdateAsync(game, ct);

                return Results.NoContent();
            }).RequireAuthorization();

        group.MapDelete("/{id:guid}", 
                    async (Guid id,
                    IGameRepository repo,
                    CancellationToken ct) =>
        {
            var game = await repo.GetByIdAsync(id, ct);
            if (game is null)
            {
                return Results.NotFound();
            }

            await repo.DeleteAsync(game, ct);
            return Results.NoContent();
        })
            .RequireAuthorization(policy =>
        {
            policy.RequireRole("Admin");
        });
    }
}