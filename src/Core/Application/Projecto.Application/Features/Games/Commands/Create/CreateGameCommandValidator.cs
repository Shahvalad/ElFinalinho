namespace Projecto.Application.Features.Games.Commands.Create
{
    public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
    {
        public CreateGameCommandValidator()
        {
            RuleFor(x=>x.CreateGameDto.CoverImage)
                .NotEmpty()
                .WithMessage("Cover image is required!")
                .NotNull()
                .WithMessage("Cover image is required!")
                .Must(x => x?.Length < Math.Pow(2, 20) * 2)
                .WithMessage("Logo must not exceed 2MB.");

            RuleFor(x=>x.CreateGameDto.Description)
                .MaximumLength(350)
                .WithMessage("Description must not exceed 200 characters.");

            RuleFor(x=>x.CreateGameDto.Name)
                .NotEmpty()
                .WithMessage("Name is required!")
                .NotNull()
                .WithMessage("Name is required!")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters!")
                .MinimumLength(1)
                .WithMessage("Name must be at least 1 character long!");

            RuleFor(x => x.CreateGameDto.Price)
                .NotEmpty().WithMessage("Price is required!")
                .GreaterThanOrEqualTo(1).WithMessage("Price must be at least 1 USD!");

            RuleFor(x => x.CreateGameDto.DeveloperId)
                .NotEmpty().WithMessage("Game cannot be created without developer!")
                .GreaterThan(0).WithMessage("DeveloperId must be positive number!");
        }
    }
}
