using GameStore.Api.Database;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;

public class GameRepository(GameStoreContext context) : IGameRepository
{
    public async Task<IEnumerable<Game>> GetAllAsync(CancellationToken ct)
    {
        return await context.Games.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Game?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id, ct);
    }

    public async Task AddAsync(Game game, CancellationToken ct)
    {
        await context.AddAsync(game, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Game game, CancellationToken ct)
    {
        context.Games.Update(game);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Game game, CancellationToken ct)
    {
        context.Games.Remove(game);
        await context.SaveChangesAsync(ct);
    }
}