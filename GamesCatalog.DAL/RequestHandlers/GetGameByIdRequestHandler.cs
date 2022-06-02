using MessagePipe;
using GamesCatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using GamesCatalog.DAL.DTO;
using GamesCatalog.DAL.Models;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class GetGameByIdRequestHandler : BaseRequestHandler, IAsyncRequestHandler<GameByIdRequest, GameResponse>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public GetGameByIdRequestHandler(GamesCatalogDbContext db) : base(db) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public ValueTask<GameResponse> InvokeAsync(GameByIdRequest request, CancellationToken cancellationToken = default)
=> new(db.Games.Include(g => g.Developer).Include(g => g.Genres).Where(g => g.Id == request.Id).Select(g => new GameResponse(g.Id, g.Name, g.Developer.Name, g.Genres.Select(genre => genre.Name))).FirstOrDefaultAsync(cancellationToken));
}
