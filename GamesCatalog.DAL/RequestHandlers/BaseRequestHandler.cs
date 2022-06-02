using GamesCatalog.DAL.DTO;
using GamesCatalog.DAL.Models;

using GamesCatalogAPI.Models;

using System.Threading;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class BaseRequestHandler
{
    protected GamesCatalogDbContext db;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public BaseRequestHandler(GamesCatalogDbContext db) => this.db = db;

    public async Task<Game> InvokeInTransaction(UpdateGameRequest gameRequest, CancellationToken cancellationToken)
    {
        Game game;
        using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            game = await InvokeInTransactionCore(gameRequest, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        return game;
    }

    protected virtual async Task<Game> InvokeInTransactionCore(UpdateGameRequest gameRequest, CancellationToken cancellationToken)
    {
        return default;
    }
}
