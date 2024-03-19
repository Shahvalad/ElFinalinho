namespace Projecto.Application.Features.Publishers.Commands.EditPublisher
{
    public class EditPublisherCommandValidator : AbstractValidator<EditPublisherCommand>
    {
        public EditPublisherCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Id is required!")
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0!");

            RuleFor(x => x.PublisherDto.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .NotNull()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.PublisherDto.Description)
                .MaximumLength(500)
                .WithMessage("Description length must not exceed 500 symbols");

            RuleFor(x => x.PublisherDto.Logo)
                .Must(x => x?.Length < Math.Pow(2, 20) * 2)
                .WithMessage("Logo must not exceed 2MB.");
        }
    }
}
