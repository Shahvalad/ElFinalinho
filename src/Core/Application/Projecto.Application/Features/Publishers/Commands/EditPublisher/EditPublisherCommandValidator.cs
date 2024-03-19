using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Publishers.Commands.EditPublisher
{
    public class EditPublisherCommandValidator : AbstractValidator<EditPublisherCommand>
    {
        public EditPublisherCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotNull()
                .WithMessage("Id is required!")
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0!");

            RuleFor(x => x.PublisherDto.Name)
                .NotNull()
                .WithMessage("Name is required!")
                .MaximumLength(50)
                .WithMessage("Name length must not exceed 50 symbols");

            RuleFor(x => x.PublisherDto.Description)
                .MaximumLength(500)
                .WithMessage("Description length must not exceed 500 symbols");
        }
    }
}
