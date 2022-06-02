using MessagePipe;
using GamesCatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using GamesCatalog.DAL.DTO;
using GamesCatalog.DAL.Models;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class GetGamesByGenreRequestHandler : BaseRequestHandler, IRequestHandler<GamesByGenreRequest, GamesByGenreResponse>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public GetGamesByGenreRequestHandler(GamesCatalogDbContext db) : base(db) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public GamesByGenreResponse Invoke(GamesByGenreRequest request)
    {
        var games = db.Games.Include(g => g.Developer).Include(g => g.Genres).Where(g => g.Genres.Any(genre => genre.Name == request.Genre))
                    .Select(g => new GameResponse(g.Id, g.Name, g.Developer.Name, g.Genres.Select(genre => genre.Name))).AsAsyncEnumerable();
        return new GamesByGenreResponse(games);
    }
}
