using GamesCatalogAPI.Models;

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
}
