using System.Runtime.CompilerServices;

namespace GamesCatalog.DAL.DTO;

public record GamesByGenreResponse(IAsyncEnumerable<GameResponse> Games);


