namespace Projecto.Application.Features.Games.Commands.Edit
{
    public class EdtGameCommandValidator : AbstractValidator<EditGameCommand>
    {
        public EdtGameCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotNull().WithMessage("Id is required!")
                .NotEmpty().WithMessage("Id is required!")
                .GreaterThan(0).WithMessage("Id must be positive number!");

            RuleFor(x => x.UpdateGameDto.CoverImage)
                .Must(x => x?.Length < Math.Pow(2, 20) * 2 || x==null)
                .WithMessage("Logo must not exceed 2MB.");

            RuleFor(x => x.UpdateGameDto.Description)
                .MaximumLength(350)
                .WithMessage("Description must not exceed 350 characters.");

            RuleFor(x => x.UpdateGameDto.Name)
                .NotEmpty()
                .WithMessage("Name is required!")
                .NotNull()
                .WithMessage("Name is required!")
                .MaximumLength(50);

            RuleFor(x => x.UpdateGameDto.Price)
                .NotEmpty().WithMessage("Price is required!")
                .GreaterThan(1).WithMessage("Price must be at least 1 USD!");

            RuleFor(x => x.UpdateGameDto.DeveloperId)
                .NotEmpty().WithMessage("Game cannot be created without developer!")
                .GreaterThan(0).WithMessage("DeveloperId must be positive number!");

        }
    }
}
