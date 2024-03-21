namespace Projecto.Application.Features.Developers.Commands.Delete
{
    public class DeleteDeveloperCommandValidator : AbstractValidator<DeleteDeveloperCommand>
    {
        public DeleteDeveloperCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Id is required")
                .GreaterThan(0)
                .WithMessage("Id must be a positive integer.");
        }
    }
}
