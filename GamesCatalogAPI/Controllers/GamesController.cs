using GamesCatalog.DAL.DTO;

using MessagePipe;

using Microsoft.AspNetCore.Mvc;

namespace GamesCatalogAPI.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
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
    [HttpGet("gentre/{gentre}")]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetByGentre(string gentre, [FromServices] IAsyncRequestHandler<GamesByGenreRequest, GamesByGenreResponse> handler, CancellationToken cancellationToken)
    {
        var gamesResponse = await handler.InvokeAsync(new GamesByGenreRequest(gentre), cancellationToken);
        return Ok(gamesResponse.Games);
    }

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
        var res = handler.InvokeAsync(value, cancellationToken);
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
        var gameResponse = handler.InvokeAsync(new UpdateGameRequest(id, request.Name, request.Developer, request.Genres), cancellationToken);
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
