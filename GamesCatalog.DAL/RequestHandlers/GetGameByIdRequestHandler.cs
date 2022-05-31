using MessagePipe;
using GamesCatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using GamesCatalog.DAL.DTO;

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
    public async ValueTask<GameResponse> InvokeAsync(GameByIdRequest request, CancellationToken cancellationToken = default)
    {
        if (request?.Id == Guid.Empty)
            throw new ArgumentException(nameof(request));

        var game = await db.Games.Include(g => g.Developer).Include(g => g.Genres).FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
        return (GameResponse)game;
    }
}
