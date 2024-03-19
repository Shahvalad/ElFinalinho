namespace Projecto.Application.Features.Publishers.Commands.CreatePublisher
{
    public class CreatePublisherCommandValidator : AbstractValidator<CreatePublisherCommand>
    {
        public CreatePublisherCommandValidator()
        {
            RuleFor(x=>x.PublisherDto.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .NotNull()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(x=>x.PublisherDto.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.PublisherDto.Logo)
                .Must(x => x?.Length < Math.Pow(2, 20) * 2)
                .WithMessage("Logo must not exceed 2MB.");
        }
    }
}
