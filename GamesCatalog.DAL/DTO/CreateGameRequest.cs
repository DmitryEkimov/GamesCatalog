namespace GamesCatalog.DAL.DTO;

public record CreateGameRequest(Guid Id, string Name, string Developer, string[] Genres);

