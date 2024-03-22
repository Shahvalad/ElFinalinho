namespace Projecto.Application.Features.Games.Commands.Delete
{
    public class DeleteGameImageCommandValidator : AbstractValidator<DeleteGameImageCommand>
    {
        public DeleteGameImageCommandValidator()
        {
            RuleFor(x=>x.FileName)
                .NotEmpty().WithMessage("FileName is required!")
                .NotNull().WithMessage("FileName is required!")
                .MaximumLength(100).WithMessage("FileName must not exceed 100 characters!");
        }
    }
}
