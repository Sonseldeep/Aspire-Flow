using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync(CancellationToken ct);
    Task<Game?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Game game, CancellationToken ct);
    Task UpdateAsync(Game game, CancellationToken ct);
    Task DeleteAsync(Game game, CancellationToken ct);
}