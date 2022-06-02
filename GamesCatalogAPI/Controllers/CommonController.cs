using System.Reflection;

using GamesCatalogAPI.Models;

using Microsoft.AspNetCore.Mvc;

namespace GamesCatalogAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class CommonController : ControllerBase
{
    /// <summary>
    /// Get versions.
    /// </summary>
    /// <remarks>Возвращает актуальные версии продуктов.</remarks>
    [HttpGet("versions")]
    public VersionsResponse GetVersions() => new(Assembly.GetEntryAssembly().GetName().Version.ToString());
}