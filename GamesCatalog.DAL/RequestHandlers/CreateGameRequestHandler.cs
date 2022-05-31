using MessagePipe;
using GamesCatalogAPI.Models;
using GamesCatalog.DAL.DTO;
using GamesCatalog.DAL.Models;
using GamesCatalog.DAL.Extensions;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class CreateGameRequestHandler : BaseCreateOrUpdateGameRequestHandler, IAsyncRequestHandler<CreateGameRequest, CreateGameResponse>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public CreateGameRequestHandler(GamesCatalogDbContext db) : base(db) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="Exception"></exception>
    public async ValueTask<CreateGameResponse> InvokeAsync(CreateGameRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request?.Name))
            throw new ArgumentException("name is required fied");

        if (string.IsNullOrEmpty(request.Developer))
            throw new ArgumentException("developer is required fied");

        Guid id;
        using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            id = GuidGenerator.GenerateNotCryptoQualityGuid();
            var game = new Game()
            {
                Id = id,
                Name = request.Name,
                Developer = await GetOrCreateDeveloper(request.Developer, cancellationToken),
                Genres = await GetOrCreateGenres(request.Genres, cancellationToken)
            };


            await db.Games.AddAsync(game, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        return new CreateGameResponse(id);
    }
}
