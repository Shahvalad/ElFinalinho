using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Developers.Commands.Create
{
    public class CreateDeveloperCommandValidator : AbstractValidator<CreateDeveloperCommand>
    {
        public CreateDeveloperCommandValidator()
        {
            RuleFor(x => x.DeveloperDto.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .NotNull()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.DeveloperDto.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.DeveloperDto.Logo)
                .Must(x => x?.Length < Math.Pow(2,20) * 2)
                .WithMessage("Logo must not exceed 2MB.");
        }
    }
}
