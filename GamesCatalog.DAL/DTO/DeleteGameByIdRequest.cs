using FluentValidation;

namespace GamesCatalog.DAL.DTO;

public record DeleteGameByIdRequest(Guid Id);

public class DeleteGameByIdRequestValidator : AbstractValidator<DeleteGameByIdRequest>
{
    public DeleteGameByIdRequestValidator()
    {
        RuleFor(r => r.Id).Must(id => id != Guid.Empty);
    }
}