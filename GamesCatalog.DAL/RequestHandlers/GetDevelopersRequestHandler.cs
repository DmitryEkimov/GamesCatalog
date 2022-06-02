using MessagePipe;
using GamesCatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using GamesCatalog.DAL.DTO;

namespace GamesCatalog.DAL.RequestHandlers;

/// <summary>
/// 
/// </summary>
public class GetDevelopersRequestHandler : BaseRequestHandler, IRequestHandler<GetDevelopersRequest, GetDevelopersResponse>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public GetDevelopersRequestHandler(GamesCatalogDbContext db) : base(db) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public GetDevelopersResponse Invoke(GetDevelopersRequest request)
    {
        var genres = db.Developers.Select(g => g.Name).AsAsyncEnumerable();
        return new GetDevelopersResponse(genres);
    }
}
