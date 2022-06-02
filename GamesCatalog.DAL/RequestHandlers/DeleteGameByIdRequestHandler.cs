using MessagePipe;
using GamesCatalogAPI.Models;
using GamesCatalog.DAL.DTO;
using GamesCatalog.DAL.Models;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class DeleteGamesByIdRequestHandler : BaseRequestHandler, IAsyncRequestHandler<DeleteGameByIdRequest, DeleteGameResponse>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public DeleteGamesByIdRequestHandler(GamesCatalogDbContext db) : base(db) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public async ValueTask<DeleteGameResponse> InvokeAsync(DeleteGameByIdRequest request, CancellationToken cancellationToken = default)
    {
        var game = new Game() { Id = request.Id };
        db.Games.Attach(game);
        db.Games.Remove(game);
        await db.SaveChangesAsync(cancellationToken);
        return new DeleteGameResponse(IsSuccess: true);
    }
}
