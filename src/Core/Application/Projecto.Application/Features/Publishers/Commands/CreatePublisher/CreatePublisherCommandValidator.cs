using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Publishers.Commands.CreatePublisher
{
    public class CreatePublisherCommandValidator : AbstractValidator<CreatePublisherCommand>
    {
        public CreatePublisherCommandValidator()
        {
            RuleFor(x=>x.PublisherDto.Name)
                .NotNull()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters");
            RuleFor(x=>x.PublisherDto.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters");
        }
    }
}
