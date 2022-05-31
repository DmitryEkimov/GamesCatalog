using System.Reflection;

using GamesCatalogAPI.Models;

using Microsoft.AspNetCore.Mvc;

namespace GamesCatalogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly ILogger _logger;

        public CommonController(ILogger<CommonController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get versions.
        /// </summary>
        /// <remarks>Возвращает актуальные версии продуктов.</remarks>
        [HttpGet("/api/v1/versions")]
        public VersionsResponse GetVersions() => new(Assembly.GetEntryAssembly().GetName().Version.ToString());
    }
}