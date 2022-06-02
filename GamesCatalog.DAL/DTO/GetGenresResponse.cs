using System.Runtime.CompilerServices;

namespace GamesCatalog.DAL.DTO;

public record GetGenresResponse(IAsyncEnumerable<string> Genres);//ConfiguredCancelableAsyncEnumerable

