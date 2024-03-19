namespace Projecto.Application.Features.Publishers.Commands.DeletePublisher
{
    public class DeletePublisherCommandValidator : AbstractValidator<DeletePublisherCommand>
    {
        public DeletePublisherCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotNull()
                .WithMessage("Id is required!")
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0!");
        }
    }
}
