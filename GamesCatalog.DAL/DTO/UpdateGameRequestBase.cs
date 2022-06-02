using FluentValidation;

namespace GamesCatalog.DAL.DTO;

public record UpdateGameRequestBase(string Name, string Developer, string[] Genres);

public class UpdateGameRequestBaseValidator : AbstractValidator<UpdateGameRequestBase>
{
    public UpdateGameRequestBaseValidator()
    {
        RuleFor(r => r.Name).Must(name => name is null || name.Length <= 36).WithMessage("field must be less than 37 symbols");
        RuleFor(r => r.Developer).Must(developer => developer is null || developer.Length <= 36).WithMessage("field must be less than 37 symbols");
        RuleFor(r => r.Genres).Must(genres => genres is null || genres.All(g => !string.IsNullOrEmpty(g) && g.Length <= 25)).WithMessage("each genres must be less than 26 symbols");
    }
}