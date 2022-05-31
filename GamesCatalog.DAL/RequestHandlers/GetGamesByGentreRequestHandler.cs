using MessagePipe;
using GamesCatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using GamesCatalog.DAL.DTO;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class GetGamesByGenreRequestHandler : BaseRequestHandler, IAsyncRequestHandler<GamesByGenreRequest, GamesByGenreResponse>
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
    public async ValueTask<GamesByGenreResponse> InvokeAsync(GamesByGenreRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request?.Genre))
            throw new ArgumentException("genre is required field");

        var games = await db.Games.Include(g => g.Developer).Include(g => g.Genres).Where(g => g.Genres.Any(genre => genre.Name == request.Genre)).ToArrayAsync(cancellationToken);
        return new GamesByGenreResponse(games.Select(g => (GameResponse)g));
    }
}
