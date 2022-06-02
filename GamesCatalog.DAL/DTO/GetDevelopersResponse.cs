using System.Runtime.CompilerServices;

namespace GamesCatalog.DAL.DTO;

public record GetDevelopersResponse(IAsyncEnumerable<string> Genres);

