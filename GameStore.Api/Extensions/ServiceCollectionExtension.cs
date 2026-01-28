using GameStore.Api.Database;
using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPresistence(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<GameStoreContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IGameRepository, GameRepository>();
        return services;
    }
}