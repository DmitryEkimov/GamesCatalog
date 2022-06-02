using MessagePipe;
using GamesCatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using GamesCatalog.DAL.DTO;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class GetGenresRequestHandler : BaseRequestHandler, IRequestHandler<GetGenresRequest, GetGenresResponse>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public GetGenresRequestHandler(GamesCatalogDbContext db) : base(db) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public GetGenresResponse Invoke(GetGenresRequest request)
    {
        var genres = db.Genres.Select(g => g.Name).AsAsyncEnumerable();//.WithCancellation(request.CancellationToken);
        return new GetGenresResponse(genres);
    }
}
