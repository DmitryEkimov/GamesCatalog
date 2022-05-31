namespace GamesCatalog.DAL.DTO;

public record UpdateGameRequest(Guid Id, string Name, string Developer, string[] Genres) : UpdateGameRequestBase(Name, Developer, Genres);
