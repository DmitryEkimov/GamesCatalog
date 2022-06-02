using FluentValidation;

namespace GamesCatalog.DAL.DTO;

public record CreateGameRequest(string Name, string Developer, string[] Genres);

public class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
{
    public CreateGameRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("field name is required").MaximumLength(36).WithMessage("field name must be less than 37 symbols");
        RuleFor(r => r.Developer).NotEmpty().WithMessage("field developer is required").MaximumLength(36).WithMessage("field developer must be less than 37 symbols");
        RuleFor(r => r.Genres).Must(genres => genres is null || genres.All(g => !string.IsNullOrEmpty(g) && g.Length <= 25)).WithMessage("each genres must be less than 26 symbols");
    }
}

