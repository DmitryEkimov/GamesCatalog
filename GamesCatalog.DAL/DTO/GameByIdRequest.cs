using FluentValidation;

namespace GamesCatalog.DAL.DTO;

public record GameByIdRequest(Guid Id);

public class GameByIdRequestValidator : AbstractValidator<GameByIdRequest>
{
    public GameByIdRequestValidator()
    {
        RuleFor(r => r.Id).Must(id => id != Guid.Empty);
    }
}