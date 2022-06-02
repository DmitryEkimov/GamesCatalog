using GamesCatalog.DAL.Models;

namespace GamesCatalog.DAL.DTO;

public record GameResponse(Guid Id, string Name, string Developer, IEnumerable<string> Genres)
{
    public static explicit operator GameResponse(Game game)
    => new GameResponse(game.Id, game.Name, game.Developer.Name, game.Genres.Select(genre => genre.Name));
}
