﻿using MessagePipe;
using GamesCatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using GamesCatalog.DAL.DTO;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class UpdateGameRequestHandler : BaseCreateOrUpdateGameRequestHandler, IAsyncRequestHandler<UpdateGameRequest, GameResponse>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public UpdateGameRequestHandler(GamesCatalogDbContext db) : base(db) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public async ValueTask<GameResponse> InvokeAsync(UpdateGameRequest request, CancellationToken cancellationToken = default)
    {
        if (request?.Id == Guid.Empty)
            throw new ArgumentException("Id is required field");

        GameResponse response;
        using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var game = await db.Games.Include(g => g.Developer).Include(g => g.Genres).FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (!string.IsNullOrEmpty(request.Name))
                game.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Developer))
                game.Developer = await GetOrCreateDeveloper(request.Developer, cancellationToken);

            if (request.Genres is not null && request.Genres.Any())
                game.Genres = await GetOrCreateGenres(request.Genres, cancellationToken);

            db.Games.Update(game);
            await transaction.CommitAsync(cancellationToken);
            response = (GameResponse)game;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        return response;
    }
}
