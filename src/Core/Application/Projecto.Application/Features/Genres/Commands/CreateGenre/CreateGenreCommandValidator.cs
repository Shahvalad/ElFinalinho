using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Genres.Commands.CreateGenre
{
    public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreCommandValidator()
        {
            RuleFor(x => x.GenreDto.Name)
                .NotNull()
                .WithMessage("Name is required.")
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters.");

            RuleFor(x=>x.GenreDto.Description)
                .MaximumLength(100)
                .WithMessage("Description must not exceed 100 characters.");

        }
    }
}
