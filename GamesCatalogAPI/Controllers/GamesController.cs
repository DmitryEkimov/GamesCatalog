using GamesCatalog.DAL.DTO;

using MessagePipe;

using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GamesCatalogAPI.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class GamesController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gentre"></param>
    /// <param name="handler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    // GET: api/<GamesController>
    [HttpGet("gentre/{genre}")]
    public IAsyncEnumerable<GameResponse> GetByGenre([Required] string genre, [FromServices] IRequestHandler<GamesByGenreRequest, GamesByGenreResponse> handler, CancellationToken cancellationToken)
    => handler.Invoke(new GamesByGenreRequest(genre, cancellationToken)).Games;

    [HttpGet("genres")]
    [ProducesResponseType(typeof(IEnumerable<string>), 200)]
    public IAsyncEnumerable<string> GetGenresList([FromServices] IRequestHandler<GetGenresRequest, GetGenresResponse> handler, CancellationToken cancellationToken)
    => handler.Invoke(new GetGenresRequest(cancellationToken)).Genres;

    [HttpGet("developers")]
    [ProducesResponseType(typeof(IEnumerable<string>), 200)]
    public IAsyncEnumerable<string> GetDevelopersList([FromServices] IRequestHandler<GetDevelopersRequest, GetDevelopersResponse> handler, CancellationToken cancellationToken)
    => handler.Invoke(new GetDevelopersRequest(cancellationToken)).Genres;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="handler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    // GET api/<GamesController>/BCA277DCA6964E549E6B65C2C9654904
    [HttpGet("{id}")]
    public async Task<ActionResult<GameResponse>> Get(Guid id, [FromServices] IAsyncRequestHandler<GameByIdRequest, GameResponse> handler, CancellationToken cancellationToken)
    {
        var gameResponse = await handler.InvokeAsync(new GameByIdRequest(id), cancellationToken);
        return Ok(gameResponse);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="handler"></param>
    /// <param name="cancellationToken"></param>
    // POST api/<GamesController>
    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromBody] CreateGameRequest value, [FromServices] IAsyncRequestHandler<CreateGameRequest, CreateGameResponse> handler, CancellationToken cancellationToken)
    {
        var res = await handler.InvokeAsync(value, cancellationToken);
        return Ok(res);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="handler"></param>
    /// <param name="cancellationToken"></param>
    // PUT api/<GamesController>/BCA277DCA6964E549E6B65C2C9654904
    [HttpPut("{id}")]
    public async Task<ActionResult<GameResponse>> Put(Guid id, [FromBody] UpdateGameRequestBase request, [FromServices] IAsyncRequestHandler<UpdateGameRequest, GameResponse> handler,
        CancellationToken cancellationToken)
    {
        var gameResponse = await handler.InvokeAsync(new UpdateGameRequest(id, request.Name, request.Developer, request.Genres), cancellationToken);
        return Ok(gameResponse);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="handler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    // DELETE api/<GamesController>/BCA277DCA6964E549E6B65C2C9654904
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(Guid id, [FromServices] IAsyncRequestHandler<DeleteGameByIdRequest, DeleteGameResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.InvokeAsync(new DeleteGameByIdRequest(id), cancellationToken);
        return Ok(result.IsSuccess);
    }
}
