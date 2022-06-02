using GamesCatalog.DAL.Extensions;
using GamesCatalog.DAL.Models;

using GamesCatalogAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class BaseCreateOrUpdateGameRequestHandler : BaseRequestHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public BaseCreateOrUpdateGameRequestHandler(GamesCatalogDbContext db) : base(db) { }

    /// <exception cref="OperationCanceledException"></exception>
    /// <summary>
    /// Gets the or create developer.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A ValueTask.</returns>
    protected async ValueTask<Developer> GetOrCreateDeveloper(string name, CancellationToken cancellationToken)
    {
        var developer = db.Developers.FirstOrDefault(d => d.Name == name);
        if (developer is not null)
            return developer;

        // add developer to db
        developer = new Developer()
        {
            Id = GuidGenerator.GenerateNotCryptoQualityGuid(),
            Name = name
        };
        await db.Developers.AddAsync(developer, cancellationToken);
        return developer;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="genresNames"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    protected async ValueTask<ICollection<Genre>> GetOrCreateGenres(string[] genresNames, CancellationToken cancellationToken)
    {
        if (genresNames is null || !genresNames.Any())
            return null;

        var genres = new Genre[genresNames.Length];

        for (var i = 0; i < genresNames.Length; i++)
        {
            genres[i] = await GetOrCreateGenre(genresNames[i], cancellationToken);
        }
        return genres;
    }

    /// <exception cref="OperationCanceledException"></exception>
    /// <summary>
    /// Gets the or create genre.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A ValueTask.</returns>
    protected async ValueTask<Genre> GetOrCreateGenre(string name, CancellationToken cancellationToken)
    {
        var genre = await db.Genres.FirstOrDefaultAsync(d => d.Name == name, cancellationToken);
        if (genre is not null)
            return genre;

        // add developer to db
        genre = new Genre()
        {
            Id = GuidGenerator.GenerateNotCryptoQualityGuid(),
            Name = name
        };
        await db.Genres.AddAsync(genre, cancellationToken);

        return genre;
    }
}
