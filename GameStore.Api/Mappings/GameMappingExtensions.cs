using GameStore.Api.DTOs;
using GameStore.Api.Entities;

namespace GameStore.Api.Mappings;

public static class GameMappingExtensions
{
   public static GameDto ToDto(this Game game) 
       => new(
        game.Id,
        game.Name,
        game.Genre,
        game.Price,
        game.ReleaseDate,
        game.ImageUri
      );

   public static Game ToEntity(this CreateGameDto dto) => new()
   {
        Id = Guid.CreateVersion7(),
        Name = dto.Name,
        Genre = dto.Genre,
        Price = dto.Price,
        ReleaseDate = dto.ReleaseDate,
        ImageUri = dto.ImageUri
        
   };

   public static void UpdateFromDto(this Game game, UpdateGameDto dto)
   {
       game.Name = dto.Name;
       game.Genre = dto.Genre;
       game.Price = dto.Price;
       game.ReleaseDate = dto.ReleaseDate;
       game.ImageUri = dto.ImageUri;
                    
   }
}