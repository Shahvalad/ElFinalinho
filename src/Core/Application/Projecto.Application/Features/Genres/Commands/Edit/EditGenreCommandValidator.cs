

namespace Projecto.Application.Features.Genres.Commands.Edit
{
    public class EditGenreCommandValidator : AbstractValidator<EditGenreCommand>
    {
        public EditGenreCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotNull()
                .WithMessage("Id is required")
                .GreaterThan(0)
                .WithMessage("Id must be a positive integer.");

            RuleFor(x=>x.GenreDto.Name)
                .NotNull()
                .WithMessage("Name is required!")
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters.");

            RuleFor(x=>x.GenreDto.Description)
                .MaximumLength(100)
                .WithMessage("Description must not exceed 100 characters.");
        }
    }
}
