using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Database;

public sealed class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>()
            .Property(g => g.Price)
            .HasPrecision(18, 2);
        
        
        modelBuilder.Entity<Game>().HasData(
            new Game
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "GTA V",
                Price = 49.99M,
                ReleaseDate = new DateOnly(2013,
                    9,
                    17),
                Genre = "fighting",
                ImageUri = "https://example.com/fortnite.jpg"
            },
            new Game
            {
                Id =  Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "FIFA 23",
                Price = 59.99M,
                ReleaseDate = new DateOnly(2022,
                    9,
                    30),
                Genre = "MOBO",
                ImageUri = "https://example.com/fortnite.jpg"
            }
           
        );
    }
}