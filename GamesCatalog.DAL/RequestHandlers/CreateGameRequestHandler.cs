using MessagePipe;
using GamesCatalogAPI.Models;
using GamesCatalog.DAL.DTO;
using GamesCatalog.DAL.Models;
using GamesCatalog.DAL.Extensions;
using System.Threading;
using Azure.Core;

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
        var id = GuidGenerator.GenerateNotCryptoQualityGuid();
        var game = await InvokeInTransaction(new UpdateGameRequest(id, request.Name, request.Developer, request.Genres), cancellationToken);
        return new CreateGameResponse(game.Id);
    }

    protected override async Task<Game> InvokeInTransactionCore(UpdateGameRequest gameViewModel, CancellationToken cancellationToken)
    {
        var game = new Game()
        {
            Id = gameViewModel.Id,
            Name = gameViewModel.Name,
            Developer = await GetOrCreateDeveloper(gameViewModel.Developer, cancellationToken),
            Genres = await GetOrCreateGenres(gameViewModel.Genres.ToArray(), cancellationToken)
        };
        await db.Games.AddAsync(game, cancellationToken);
        return game;
    }
}
