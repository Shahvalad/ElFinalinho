namespace Projecto.Application.Features.Games.Commands.Delete
{
    public class DeleteGameCommandValidator : AbstractValidator<DeleteGameCommand>
    {
        public DeleteGameCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotNull()
                .WithMessage("Id is required!")
                .GreaterThan(0)
                .WithMessage("Id must be positive number!");
        }
    }
}
